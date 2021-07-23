using ExpanseManager.ConsoleView;
using ExpanseManager.Controller.Services.Accounts;
using ExpanseManagerDBLibrary.Models;
using ExpanseManagerDBLibrary.Repositories.Accounts;
using ExpanseManagerDBLibrary.Repositories.Currencies;
using ExpanseManagerDBLibrary.Repositories.CurrencyConversions;
using ExpanseManagerDBLibrary.Repositories.Payments;
using ExpanseManagerServiceLibrary.Services.Accounts;
using ExpanseManagerServiceLibrary.Services.Currencies;
using ExpanseManagerServiceLibrary.Services.CurrencyConversions;
using ExpanseManagerServiceLibrary.Services.Payments;
using ExpanseManagerServiceLibrary.Services.Security;
using ExpanseManagerServiceLibrary.Services.Transactions;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace ExpanseManager.Controller
{
    public class ApplicationManager
    {
        private bool endProgram = false;

        public IPaymentRepository PaymentRepository { get; }
        public IAccountRepository AccountRepository { get; }
        public ICurrencyRepository CurrencyRepository { get; }
        public ICurrencyConversionRepository CurrencyConversionRepository { get; }
        public IPasswordService PasswordService { get; }
        public IPaymentService PaymentService { get; }
        public IAccountService AccountService { get; }
        public ICurrencyService CurrencyService { get; }
        public ICurrencyConversionService CurrencyConversionService { get; }
        public ITransactionService TransactionService { get; }
        public AccountServiceView AccountServiceView { get; }

        public ApplicationManager()
        {
            PaymentRepository = new PaymentRepositoryImpl();
            AccountRepository = new AccountRepositoryImpl();
            CurrencyRepository = new CurrencyRepositoryImpl();
            CurrencyConversionRepository = new CurrencyConversionRepositoryImpl();
            PasswordService = new PasswordService();
            PaymentService = new PaymentServiceImpl(PaymentRepository);
            AccountService = new AccountServiceImpl(AccountRepository);
            CurrencyService = new CurrencyServiceImpl(CurrencyRepository);
            CurrencyConversionService = new CurrencyConversionServiceImpl(CurrencyConversionRepository);
            TransactionService = new TransactionServiceImpl(AccountService, PaymentService, CurrencyConversionService);
            AccountServiceView = new AccountServiceView(AccountService, PasswordService, CurrencyService);
        }

        public async Task Start()
        {
            ApplicationManagerMessages.PrintWelcomeMessage();
            await CreateRootIdentityIfDoesntExist();
            BasicOutputMessages.PrintAcknowledgeMessage();
            await CommandPhase();                                     /* Account manipulation */
            ApplicationManagerMessages.PrintGoodByeMessage();
        }

        private async Task CreateRootIdentityIfDoesntExist()
        {
            if (await AccountService.GetAccountByUserNameAsync("root") == null)
            {
                ApplicationManagerMessages.PrintRootNotSetUpMessage();
                await AccountServiceView.CreateRootAccountAsync();
                ApplicationManagerMessages.PrintAccountAddedMessage();
            }
        }

        private async Task CommandPhase()
        {
            while (!endProgram)
            {
                Console.Clear();
                ApplicationManagerMessages.PrintApplicationCommands();
                Console.WriteLine();
                Console.Write("Command: ");
                string userInput = Console.ReadLine();
                await HandleUserDecision(userInput);
            }
        }

        private async Task HandleUserDecision(string userDecision)
        {
            Console.WriteLine();
            switch (userDecision)
            {
                case "quit":
                    endProgram = true;
                    break;
                case "new":
                    await AccountServiceView.CreateNewAccountAsync();
                    BasicOutputMessages.PrintAcknowledgeMessage();
                    break;
                case "login":
                    await LogIn();
                    break;
                case "root":
                    var root = await AccountService.GetAccountByUserNameAsync("root");
                    Console.WriteLine(root.ToJSON());
                    BasicOutputMessages.PrintAcknowledgeMessage();
                    break;
                case "users":
                    /*  Prints accounts info in JSON (just for debug version) */
                    await PrintAccounts();
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

        private async Task LogIn()
        {
            Console.Clear();
            BasicOutputMessages.PrintResponseMessage("Insert UserName of account.");
            Console.Write("Username: ");
            var username = Console.ReadLine().Trim();

            Console.WriteLine();
            BasicOutputMessages.PrintResponseMessage($"Insert password for user {username}");
            Console.Write("Password: ");
            var password = ViewUtils.GetHiddenConsoleInput();

            Console.WriteLine();

            AccountModel account = await AccountService.GetAccountByUserNameAsync(username);
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
            var accountManager = await AccountManager.InitializeAsync(account, AccountServiceView, TransactionService);
            await accountManager.StartAsync();
        }

        public async Task PrintAccounts()
        {
            BasicOutputMessages.PrintResponseMessage("Available accounts:");
            Console.WriteLine();
            Console.WriteLine(JsonConvert.SerializeObject(await AccountService.GetAllAccountsAsync(), Formatting.Indented));
        }
    }
}
