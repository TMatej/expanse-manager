using ExpanseManager.ConsoleView;
using ExpanseManager.Controller.Accounts;
using ExpanseManagerDBLibrary.Models;
using System;
using System.Threading.Tasks;

namespace ExpanseManager.Controller.Services.Accounts
{
    class AccountEditor
    {
        private bool confirmEditing = false;

        public AccountModel Account { get; }
        public AccountModel NewAccount { get; }
        public AccountServiceView AccountServiceView { get; }
        
        public AccountEditor(AccountModel account, AccountServiceView accountService)
        {
            Account = account;
            AccountServiceView = accountService;
            NewAccount = new AccountModel();
            AccountUtils.CoppyAccount(Account, NewAccount);
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
                    NewAccount.UserName = await AccountServiceView.CreateUserNameAsync("Choose new unique username. Unfortunately, it cannot be your present name.");
                    BasicOutputMessages.PrintSuccessMessage("Success!");
                    BasicOutputMessages.PrintAcknowledgeMessage();
                    break;
                case "password":
                    BasicOutputMessages.PrintResponseMessage("Your current password is: ... You should remember it mate :D ");
                    NewAccount.PasswordHash = AccountServiceView.CreatePassword();
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
                    NewAccount.Currency = await AccountServiceView.ChooseCurrencyAsync("Pick new currency for your account. If none of the following is picked, first listed currency is used.");
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
                    if (await AccountServiceView.AccountService.UpdateAccountAsync(NewAccount))
                    {
                        AccountUtils.CoppyAccount(NewAccount, Account);
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
