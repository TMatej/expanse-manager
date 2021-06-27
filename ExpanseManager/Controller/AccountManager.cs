using ExpanseManager.ConsoleView;
using ExpanseManager.Controller.Services.Accounts;
using ExpanseManagerDBLibrary.Models;
using System;

namespace ExpanseManager.Controller
{
    public class AccountManager
    {
        public Account ManagedAccount { get; }
        public AccountServiceView AccountServiceView { get; }

        private bool logout = false;

        public AccountManager(Account account, AccountServiceView accountService)
        {
            ManagedAccount = account;
            ManagedAccount.LastTimeLogedIn = DateTime.Now;
            AccountServiceView = accountService;
            AccountServiceView.UpdateAccount(ManagedAccount);
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
                    //Pay();
                    BasicOutputMessages.PrintResponseMessage("Sorry, this feature is not implemented yet. You'll have to wait till another release :/.");
                    BasicOutputMessages.PrintAcknowledgeMessage();
                    break;
                case "change":
                    var editor = new AccountEditor(ManagedAccount, AccountServiceView);
                    editor.Start();
                    break;
                case "export":
                    break;
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
            /* show your ballance */
            /* show available users */
            /* pick user */
            /* propose payment -> TransactionService.Transferballance(u1, u2, amount) */
            /* propagate the changes in local objects*/
        }

        public void LogOut()
        {
            logout = true;
            ManagedAccount.LastTimeLogedIn = DateTime.Now;
        }

        public string ToJSON()
        {
            if (ManagedAccount is Account account)
            {
                return account.ToJSON();
            }

            return "No JSON representation available :(";
        }
    }
}