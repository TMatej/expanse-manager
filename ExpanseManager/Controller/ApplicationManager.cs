using ExpanseManager.ConsoleView;
using ExpanseManager.Controller.Services.Accounts;
using ExpanseManagerDBLibrary.Models;
using ExpanseManagerDBLibrary.Repositories.Accounts;
using ExpanseManagerDBLibrary.Repositories.Currencies;
using ExpanseManagerDBLibrary.Repositories.CurrencyConversions;
using ExpanseManagerDBLibrary.Repositories.Payments;
using ExpanseManagerServiceLibrary.Services.Accounts;
using ExpanseManagerServiceLibrary.Services.Currencies;
using ExpanseManagerServiceLibrary.Services.Security;
using Newtonsoft.Json;
using System;

/*TODO ALL data should be store after user successfuly logs out!*/

namespace ExpanseManager.Controller
{
    public class ApplicationManager
    {
        private bool endProgram = false;

        public IPaymentRepository PaymentRepository;
        public IAccountRepository AccountRepository;
        public ICurrencyRepository CurrencyRepository;
        public ICurrencyConversionRepository CurrencyConversionRepository;

        public IPasswordService PasswordService;
        public IAccountService AccountService;
        public ICurrencyService CurrencyService;
        public AccountServiceView AccountServiceView;

        public ApplicationManager()
        {
            PaymentRepository = new PaymentRepositoryImpl();
            AccountRepository = new AccountRepositoryImpl();
            CurrencyRepository = new CurrencyRepositoryImpl();
            CurrencyConversionRepository = new CurrencyConversionRepositoryImpl();
            PasswordService = new PasswordService();
            AccountService = new AccountServiceImpl(AccountRepository);
            CurrencyService = new CurrencyServiceImpl(CurrencyRepository);
            AccountServiceView = new AccountServiceView(AccountService, PasswordService, CurrencyService);
        }

        public void Start()
        {
            ApplicationManagerMessages.PrintWelcomeMessage();
            CreateRootIdentityIfDoesntExist();
            BasicOutputMessages.PrintAcknowledgeMessage();
            CommandPhase();                                     /* Account manipulation */
            ApplicationManagerMessages.PrintGoodByeMessage();
        }

        private void CreateRootIdentityIfDoesntExist()
        {
            if (AccountServiceView.GetAccountByUserName("root") == null)
            {
                ApplicationManagerMessages.PrintRootNotSetUpMessage();
                AccountServiceView.CreateRootAccount();
                ApplicationManagerMessages.PrintAccountAddedMessage();
            }
        }

        private void CommandPhase()
        {
            while (!endProgram)
            {
                Console.Clear();
                ApplicationManagerMessages.PrintApplicationCommands();
                Console.WriteLine();
                Console.Write("Command: ");
                string userInput = Console.ReadLine();
                HandleUserDecision(userInput);
            }
        }

        private void HandleUserDecision(string userDecision)
        {
            Console.WriteLine();
            switch (userDecision)
            {
                case "quit":
                    endProgram = true;
                    break;
                case "new":
                    AccountServiceView.CreateNewAccount();
                    BasicOutputMessages.PrintAcknowledgeMessage();
                    break;
                case "login":
                    LogIn();
                    break;
                case "root":
                    Console.WriteLine(AccountServiceView.GetAccountByUserName("root").ToJSON());
                    BasicOutputMessages.PrintAcknowledgeMessage();
                    break;
                case "users":
                    /*  Prints accounts info in JSON (just for debug version) */
                    PrintAccounts();
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

        private void LogIn()
        {
            Console.Clear();
            BasicOutputMessages.PrintResponseMessage("Insert UserName of account.");
            Console.Write("Username: ");
            var username = Console.ReadLine().Trim();

            Console.WriteLine();
            BasicOutputMessages.PrintResponseMessage($"Insert password for user {username}");
            Console.Write("Password: ");
            var password = AccountServiceView.GetHiddenConsoleInput();  // TODO -> MAKE UTILS CLASS

            Console.WriteLine();

            Account account = AccountServiceView.GetAccountByUserName(username);
            if (account == null)
            {
                BasicOutputMessages.PrintErrorMessage("Username or password is incorect! Try again.");
                BasicOutputMessages.PrintAcknowledgeMessage();
                return;
            }

            if (!PasswordService.VerifyPassword(account.PasswordHash, password))
            {
                BasicOutputMessages.PrintErrorMessage("Username or password is incorect! Try again.");
                BasicOutputMessages.PrintAcknowledgeMessage();
                return;
            }

            Console.WriteLine();
            BasicOutputMessages.PrintSuccessMessage("Login successful!");
            BasicOutputMessages.PrintAcknowledgeMessage();
            Console.Clear();
            var accountManager = new AccountManager(account, AccountServiceView);
            accountManager.Start();
        }

        public void PrintAccounts()
        {
            BasicOutputMessages.PrintResponseMessage("Available accounts:");
            Console.WriteLine();
            Console.WriteLine(JsonConvert.SerializeObject(AccountServiceView.GetAllAccounts(), Formatting.Indented));
        }
    }
}
