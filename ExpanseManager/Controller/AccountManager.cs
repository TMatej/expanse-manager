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

        public AccountManager(AccountModel account, AccountServiceView accountService, ITransactionService transactionService)
        {
            ManagedAccount = account;
            ManagedAccount.LastTimeLogedIn = DateTime.Now;
            AccountServiceView = accountService;
            AccountServiceView.UpdateAccount(ManagedAccount);
            TransactionService = transactionService;
        }

        public void Start()
        {
            CommandPhase();            
        }

        private void CommandPhase()
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
                    HandleRootDecision(rootInput.Trim());
                } else
                {
                    AccountManagerMessages.PrintAccountCommands();
                    Console.Write("\nCommand: ");
                    var userInput = Console.ReadLine();
                    Console.WriteLine();
                    HandleUserDecision(userInput.Trim());
                }
                                   
            }
        }

        private void HandleRootDecision(string userDecision)
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
                    HandleUserDecision(userDecision);
                    break;
                case "add cur":
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

        private void HandleUserDecision(string userDecision)
        {
            switch (userDecision)
            {
                case "logout":
                    LogOut();
                    break;
                case "add":
                    Add();
                    break;
                case "pay":
                    Pay();
                    BasicOutputMessages.PrintAcknowledgeMessage();
                    break;
                case "change":
                    var editor = new AccountEditor(ManagedAccount, AccountServiceView);
                    editor.Start();
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

        public void Add()
        {
            BasicOutputMessages.PrintResponseMessage("Insert desired ballance (must be greater than 0):");
            var input = Console.ReadLine();
            if (decimal.TryParse(input.Trim(), out decimal convertedInput) && convertedInput >= 0)
            {
                ManagedAccount.Ballance += convertedInput;
                AccountServiceView.UpdateAccount(ManagedAccount);
                BasicOutputMessages.PrintResponseMessage($"Ballance increased! Current ballance is {ManagedAccount.Ballance} {ManagedAccount.Currency.ShortName}");
            } 
            else
            {
                BasicOutputMessages.PrintInvalidInputErorMessage(input);
            }
            BasicOutputMessages.PrintAcknowledgeMessage();
        }

        public void Pay()
        {
            /* show available users */
            var accounts = Task.Run(async () => await AccountServiceView.AccountService.GetAllAccountsAsync()).Result;
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

            /* check currencies */
            /* show your ballance */
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
                    Task.Run(async () => 
                    {
                        try
                        {
                            var transferedAmount = await TransactionService.TransferBallance(ManagedAccount, chosenAccount, parsedAmount);
                            BasicOutputMessages.PrintResponseMessage($"{transferedAmount} {chosenAccount.Currency.ShortName} was transfered to user {chosenAccount.Name}.");
                        } 
                        catch (UnknownExchangeRateException ex)
                        {
                            BasicOutputMessages.PrintErrorMessage(ex.Message);
                            BasicOutputMessages.PrintAcknowledgeMessage();
                            return;
                        }
                    }
                    );
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