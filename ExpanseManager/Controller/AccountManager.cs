using ExpanseManager.ConsoleView;
using ExpanseManager.Controller.Services.Accounts;
using ExpanseManagerDBLibrary.Models;
using ExpanseManagerServiceLibrary.Exceptions;
using ExpanseManagerServiceLibrary.Services.Transactions;
using System;
using System.Threading.Tasks;

namespace ExpanseManager.Controller
{
    public class AccountManager
    {
        public AccountModel ManagedAccount { get; }
        public AccountServiceView AccountServiceView { get; }
        public ITransactionService TransactionService { get; } 

        private bool logout = false;

        private AccountManager(AccountModel account, AccountServiceView accountService, ITransactionService transactionService)
        {
            ManagedAccount = account;
            AccountServiceView = accountService;
            TransactionService = transactionService;
            ManagedAccount.LastTimeLogedIn = DateTime.Now;
        }

        public static async Task<AccountManager> InitializeAsync(AccountModel account, AccountServiceView accountService, ITransactionService transactionService)
        {
            var myManager = new AccountManager(account, accountService, transactionService);
            await myManager.AccountServiceView.AccountService.UpdateAccountAsync(myManager.ManagedAccount);
            return myManager;
        }

        public async Task StartAsync()
        {
            await CommandPhaseAsync();            
        }

        private async Task CommandPhaseAsync()
        {
            while (!logout)
            {
                Console.Clear();
                if (ManagedAccount.UserName.Equals("root"))
                {
                    AccountManagerMessages.PrintRootCommands();
                    Console.Write("\nCommand: ");
                    var rootInput = Console.ReadLine();
                    Console.WriteLine();
                    await HandleRootDecisionAsync(rootInput.Trim());
                } else
                {
                    AccountManagerMessages.PrintAccountCommands();
                    Console.Write("\nCommand: ");
                    var userInput = Console.ReadLine();
                    Console.WriteLine();
                    await HandleUserDecisionAsync(userInput.Trim());
                }
                                   
            }
        }

        private async Task HandleRootDecisionAsync(string userDecision)
        {
            switch (userDecision)
            {
                case "logout":                /* Quit the application */
                case "add":
                case "pay":
                case "change":
                case "export": 
                case "history":
                case "json":
                case "":
                    await HandleUserDecisionAsync(userDecision);
                    break;
                case "all cur":
                    await PrintAllCurrencies();
                    BasicOutputMessages.PrintAcknowledgeMessage();
                    break;
                case "add cur":
                    await AddCurrencyAsync();
                    break;
                case "remove cur":
                case "remove acc":
                case "revert payment":
                    BasicOutputMessages.PrintResponseMessage("Sorry, this feature is not implemented yet. You'll have to wait till another release :/.");
                    BasicOutputMessages.PrintAcknowledgeMessage();
                    break;
                default:
                    BasicOutputMessages.PrintInvalidInputCommandErorMessage(userDecision);
                    BasicOutputMessages.PrintAcknowledgeMessage();
                    break;
            }
        }

        private async Task PrintAllCurrencies()
        {
            var currencies = await AccountServiceView.CurrencyService.GetAllCurrenciesAsync();
            var position = 0;

            Console.WriteLine("Currencies: ");

            if (currencies.Count == 0)
            {
                Console.WriteLine("No currency exists!");
            }

            foreach (var curr in currencies)
            {
                position += 1;
                Console.WriteLine($"\t{position}\t{curr.Name} [{curr.ShortName}]");
            }
        }

        private async Task AddCurrencyAsync()
        {
            BasicOutputMessages.PrintResponseMessage("Insert new currency long name:");
            var longNameInput = Console.ReadLine();

            BasicOutputMessages.PrintResponseMessage("Insert new currency short name:");
            var shortNameInput = Console.ReadLine();

            try
            {
                CurrencyModel currency = new(shortNameInput.Trim(), longNameInput.Trim());
                await AccountServiceView.CurrencyService.CreateCurrency(currency);
            }
            catch (CouldntCreateCurrencyException ex)
            {
                BasicOutputMessages.PrintErrorMessage("Currency with provided long/short name already exists.");
                BasicOutputMessages.PrintErrorMessage(ex.InnerException?.Message);
            }

            BasicOutputMessages.PrintAcknowledgeMessage();
        }

        private async Task HandleUserDecisionAsync(string userDecision)
        {
            switch (userDecision)
            {
                case "logout":
                    LogOut();
                    break;
                case "add":
                    await AddAsync();
                    break;
                case "pay":
                    await PayAsync();
                    BasicOutputMessages.PrintAcknowledgeMessage();
                    break;
                case "change":
                    await StartAccountEditingAsync();
                    break;
                case "export":
                case "history":
                    BasicOutputMessages.PrintResponseMessage("Sorry, this feature is not implemented yet. You'll have to wait till another release :/.");
                    BasicOutputMessages.PrintAcknowledgeMessage();
                    break;
                case "json":
                    Console.WriteLine(ManagedAccount.ToJSON());
                    BasicOutputMessages.PrintAcknowledgeMessage();
                    break;
                case "":
                    BasicOutputMessages.PrintResponseMessage("Please, type in a command from given list above.");
                    BasicOutputMessages.PrintAcknowledgeMessage();
                    break;
                default:
                    BasicOutputMessages.PrintInvalidInputCommandErorMessage(userDecision);
                    BasicOutputMessages.PrintAcknowledgeMessage();
                    break;
            }
        }

        private async Task StartAccountEditingAsync()
        {
            var editor = new AccountEditor(ManagedAccount, AccountServiceView);
            await editor.StartAsync();
        }

        public async Task AddAsync()
        {
            BasicOutputMessages.PrintResponseMessage("Insert desired ballance (must be greater than 0):");
            var input = Console.ReadLine();
            if (decimal.TryParse(input.Trim(), out decimal convertedInput) && convertedInput >= 0)
            {
                ManagedAccount.Ballance += convertedInput;
                await AccountServiceView.AccountService.UpdateAccountAsync(ManagedAccount);
                BasicOutputMessages.PrintResponseMessage($"Ballance increased! Current ballance is {ManagedAccount.Ballance} {ManagedAccount.Currency.ShortName}");
            } 
            else
            {
                BasicOutputMessages.PrintInvalidInputErorMessage(input);
            }
            BasicOutputMessages.PrintAcknowledgeMessage();
        }

        public async Task PayAsync()
        {
            /* show available users */
            var accounts = await AccountServiceView.AccountService.GetAllAccountsAsync();
            int position = 1;

            BasicOutputMessages.PrintResponseMessage("Available accounts:");

            foreach (var account in accounts)
            {
                Console.WriteLine($"{position} - {account.UserName} - [{account.Currency.ShortName}]");
                ++position;
            }

            /* pick user */
            Console.WriteLine();
            BasicOutputMessages.PrintResponseMessage("Choose account from above list. Please insert the number associated with the account.");
            var input = Console.ReadLine();
            AccountModel chosenAccount;

            while (true)
            {
                if (int.TryParse(input.Trim(), out var parsedInput) && parsedInput > 0 && parsedInput <= accounts.Count)
                {
                    chosenAccount = accounts[parsedInput-1];
                    break;
                }
                else
                {
                    Console.WriteLine();
                    BasicOutputMessages.PrintInvalidInputErorMessage(input);
                    BasicOutputMessages.PrintResponseMessage("Choose account from above list. Please insert the number associated with the account.");
                    input = Console.ReadLine();
                }   
            }

            Console.WriteLine();
            BasicOutputMessages.PrintResponseMessage("Your available ballance is:");
            Console.WriteLine($"{ManagedAccount.Ballance} {ManagedAccount.Currency.ShortName}");
            Console.WriteLine();
            BasicOutputMessages.PrintResponseMessage("Insert the ballance you want to transfer:");
            var inputAmount = Console.ReadLine();

            while (true)
            {
                if (int.TryParse(inputAmount.Trim(), out var parsedAmount) && parsedAmount > 0 && parsedAmount <= ManagedAccount.Ballance)
                {
                    try
                    {
                        var transferedAmount = await TransactionService.TransferBallance(ManagedAccount, chosenAccount, parsedAmount);
                        BasicOutputMessages.PrintResponseMessage($"{transferedAmount} {chosenAccount.Currency.ShortName} was transfered to user {chosenAccount.Name}.");
                    } 
                    catch (UnknownExchangeRateException ex)
                    {
                        BasicOutputMessages.PrintErrorMessage(ex.Message);
                        return;
                    }
                    catch (InsufficientBallanceException ex)
                    {
                        BasicOutputMessages.PrintErrorMessage(ex.Message);
                        return;
                    }
                    break;
                }
                else
                {
                    BasicOutputMessages.PrintInvalidInputErorMessage(inputAmount);
                    BasicOutputMessages.PrintResponseMessage("Try again.");
                    inputAmount = Console.ReadLine();
                }
            }
            BasicOutputMessages.PrintSuccessMessage("Success!");
        }

        public void LogOut()
        {
            logout = true;
            ManagedAccount.LastTimeLogedIn = DateTime.Now;
        }

        public string ToJSON()
        {
            return ManagedAccount.ToJSON();
        }
    }
}