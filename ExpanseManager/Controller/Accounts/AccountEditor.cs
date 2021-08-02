using ExpanseManager.ConsoleView;
using ExpanseManager.Controller.Accounts;
using ExpanseManagerDBLibrary.Models;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace ExpanseManager.Controller.Services.Accounts
{
    class AccountEditor
    {
        private bool confirmEditing = false;

        public AccountModel Account { get; }
        public AccountModel NewAccount { get; }
        public AccountUtils AccountUtils { get; }
        
        public AccountEditor(AccountModel account, AccountUtils accountUtils)
        {
            Account = account;
            AccountUtils = accountUtils;
            NewAccount = new AccountModel();
            Controller.Accounts.AccountUtils.CoppyAccount(Account, NewAccount);
        }

        public async Task StartAsync()
        {
            await CommandPhaseAsync();
        }

        public async Task CommandPhaseAsync()
        {
            while (!confirmEditing)
            {
                Console.Clear();
                BasicOutputMessages.PrintResponseMessage("Current user information:");
                Console.WriteLine(Account.ToJSON());
                BasicOutputMessages.PrintResponseMessage("Edited user information:");
                Console.WriteLine(NewAccount.ToJSON());
                AccountEditorMessages.PrintEditingCommands();
                Console.Write("Command: ");
                var userInput = Console.ReadLine();
                await HandleUserDecisionAsync(userInput.Trim());
            }
        }

        private async Task HandleUserDecisionAsync(string userDecision)
        {
            switch (userDecision)
            {
                case "name":
                    BasicOutputMessages.PrintResponseMessage("Your current name is: ");
                    BasicOutputMessages.PrintResponseMessage(Account.Name);
                    NewAccount.Name = AccountUtils.CreateName("Insert your new name.");
                    BasicOutputMessages.PrintSuccessMessage("Success!");
                    BasicOutputMessages.PrintAcknowledgeMessage();
                    break;
                case "username":
                    if (CheckRoot())
                    {
                        return;
                    }
                    BasicOutputMessages.PrintResponseMessage("Your current username is:");
                    BasicOutputMessages.PrintResponseMessage(Account.UserName);
                    NewAccount.UserName = await AccountUtils.CreateUserNameAsync("Choose new unique username. Unfortunately, it cannot be your present name.");
                    BasicOutputMessages.PrintSuccessMessage("Success!");
                    BasicOutputMessages.PrintAcknowledgeMessage();
                    break;
                case "password":
                    BasicOutputMessages.PrintResponseMessage("Your current password is: ... You should remember it mate :D ");
                    NewAccount.PasswordHash = AccountUtils.CreatePassword();
                    BasicOutputMessages.PrintSuccessMessage("Success!");
                    BasicOutputMessages.PrintAcknowledgeMessage();
                    break;
                case "sex":
                    if (CheckRoot())
                    {
                        return;
                    }
                    BasicOutputMessages.PrintResponseMessage("Your current username is:");
                    BasicOutputMessages.PrintResponseMessage(Enum.GetName(Account.Gender));
                    NewAccount.Gender = AccountUtils.ChooseGender("Pick your new gender.");
                    BasicOutputMessages.PrintSuccessMessage("Success!");
                    BasicOutputMessages.PrintAcknowledgeMessage();
                    break;
                case "currency":
                    BasicOutputMessages.PrintResponseMessage("Your current currency is:");
                    BasicOutputMessages.PrintResponseMessage($"{Account.Currency.Name} [{Account.Currency.ShortName}]");
                    await ChangeCurrency();
                    BasicOutputMessages.PrintSuccessMessage("Success!");
                    BasicOutputMessages.PrintAcknowledgeMessage();
                    break;
                case "abort":
                    Console.Clear();
                    confirmEditing = true;
                    AccountEditorMessages.PrintEditingAbortedMessage();
                    BasicOutputMessages.PrintAcknowledgeMessage();
                    Console.Clear();
                    break;
                case "done":
                    Console.Clear();
                    confirmEditing = true;
                    if (await AccountUtils.AccountService.UpdateAccountAsync(NewAccount))
                    {
                        Controller.Accounts.AccountUtils.CoppyAccount(NewAccount, Account);
                    }

                    AccountEditorMessages.PrintEditingDoneMessage();
                    BasicOutputMessages.PrintAcknowledgeMessage();
                    Console.Clear();
                    break;
                case "":
                    BasicOutputMessages.PrintResponseMessage("Please, type in a command from given list above.");
                    BasicOutputMessages.PrintAcknowledgeMessage();
                    break;
                default:
                    Console.Clear();
                    BasicOutputMessages.PrintInvalidInputCommandErorMessage(userDecision);
                    BasicOutputMessages.PrintAcknowledgeMessage();
                    break;
            }
        }

        private async Task ChangeCurrency()
        {
            var availableConversions = await AccountUtils.CurrencyConversionService.GetCurrencyConversionsByFromCurrency(Account.Currency);
            var availableCurrencies = availableConversions.Select(conversion => conversion.CurrencyTo).ToList();

            if (availableConversions.Count == 0)
            {
                Console.WriteLine("No available conversion exists!");
                return;
            }

            Console.WriteLine("Options:");
            Console.WriteLine();
            Console.WriteLine("\tID\tName\tShort name\tConversion rate");

            var position = 0;

            foreach (var curr in availableConversions)
            {
                position += 1;
                Console.WriteLine($"\t{position}\t{curr.CurrencyTo.Name}\t[{curr.CurrencyTo.ShortName}]\t{curr.Rate} : 1");
            }

            string input;
            Console.WriteLine();
            CurrencyConversionModel conversion;
            CurrencyModel currency;

            while (true)
            {
                Console.Write("Option: ");
                input = Console.ReadLine().Trim();
                if (!int.TryParse(input, out var parsedInput) || parsedInput <= 0 || parsedInput > availableConversions.Count)
                {
                    BasicOutputMessages.PrintInvalidInputErorMessage(input);
                }
                else
                {
                    conversion = availableConversions[parsedInput - 1];
                    currency = conversion.CurrencyTo;
                    break;
                }
            }
            
            BasicOutputMessages.PrintResponseMessage($"Choosen currency: {currency.Name} [{currency.ShortName}]");
            NewAccount.Currency = currency;
            NewAccount.Ballance = decimal.Multiply(NewAccount.Ballance, conversion.Rate);
            BasicOutputMessages.PrintResponseMessage($"New account ballance is {NewAccount.Ballance} {NewAccount.Currency.ShortName}.");
            Console.WriteLine();
        }

        private bool CheckRoot()
        {
            if (Account.UserName.Equals("root"))
            {
                BasicOutputMessages.PrintErrorMessage("This property of user 'root' cannot be changed!");
                BasicOutputMessages.PrintAcknowledgeMessage();
                return true;
            }

            return false;
        } 
    }
}
