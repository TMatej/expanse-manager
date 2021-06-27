using ExpanseManager.ConsoleView;
using ExpanseManagerDBLibrary.Models;
using System;

namespace ExpanseManager.Controller.Services.Accounts
{
    class AccountEditor
    {
        private bool confirmEditing = false;

        public Account Account { get; }
        public Account NewAccount { get; }
        public AccountServiceView AccountServiceView { get; }
        
        public AccountEditor(Account account, AccountServiceView accountService)
        {
            Account = account;
            AccountServiceView = accountService;
            NewAccount = new Account();
            AccountServiceView.CoppyAccount(Account, NewAccount);
        }

        public void Start()
        {
            CommandPhase();
        }

        public void CommandPhase()
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
                HandleUserDecision(userInput.Trim());
            }
        }

        private void HandleUserDecision(string userDecision)
        {
            switch (userDecision)
            {
                case "name":
                    BasicOutputMessages.PrintResponseMessage("Your current name is: ");
                    BasicOutputMessages.PrintResponseMessage(Account.Name);
                    NewAccount.Name = AccountServiceView.CreateName("Insert your new name.");
                    Console.WriteLine("Success!");
                    BasicOutputMessages.PrintAcknowledgeMessage();
                    break;
                case "username":
                    if (CheckRoot())
                    {
                        return;
                    }
                    BasicOutputMessages.PrintResponseMessage("Your current username is:");
                    BasicOutputMessages.PrintResponseMessage(Account.UserName);
                    NewAccount.UserName = AccountServiceView.CreateUserName("Choose new unique username. Unfortunately, it cannot be your present name.");
                    Console.WriteLine("Success!");
                    BasicOutputMessages.PrintAcknowledgeMessage();
                    break;
                case "password":
                    BasicOutputMessages.PrintResponseMessage("Your current password is: ... You should remember it mate :D ");
                    NewAccount.PasswordHash = AccountServiceView.CreatePassword();
                    Console.WriteLine("Success!");
                    BasicOutputMessages.PrintAcknowledgeMessage();
                    break;
                case "sex":
                    if (CheckRoot())
                    {
                        return;
                    }
                    BasicOutputMessages.PrintResponseMessage("Your current username is:");
                    BasicOutputMessages.PrintResponseMessage(Enum.GetName(Account.Gender));
                    NewAccount.Gender = AccountServiceView.ChooseGender("Pick your new gender.");
                    Console.WriteLine("Success!");
                    BasicOutputMessages.PrintAcknowledgeMessage();
                    break;
                case "currency":
                    /*GONNA BE PAIN IN THE ASS*/
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
                    if (AccountServiceView.UpdateAccount(NewAccount))
                    {
                        AccountServiceView.CoppyAccount(NewAccount, Account);
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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("This property of user 'root' cannot be changed!");
                Console.ForegroundColor = ConsoleColor.White;
                BasicOutputMessages.PrintAcknowledgeMessage();
                return true;
            }

            return false;
        }
    }
}
