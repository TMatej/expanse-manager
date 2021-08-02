using ExpanseManager.Controller.Accounts;
using ExpanseManager.Controller.Services.Accounts;
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

namespace ExpanseManager.Controller
{
    public static class ComponentFactory
    {
        public static IPaymentRepository GetPaymentRepository()
        {
            return new PaymentRepositoryImpl();
        }

        public static IAccountRepository GetAccountRepository() 
        {
            return new AccountRepositoryImpl();
        }

        public static ICurrencyRepository GetCurrencyRepository()
        {
            return new CurrencyRepositoryImpl();
        }
        public static ICurrencyConversionRepository GetCurrencyConversionRepository()
        {
            return new CurrencyConversionRepositoryImpl();
        }
        public static IPasswordService GetPasswordService()
        {
            return new PasswordService();
        }
        public static IPaymentService GetPaymentService()
        {
            return new PaymentServiceImpl(GetPaymentRepository());
        }
        public static IAccountService GetAccountService()
        {
            return new AccountServiceImpl(GetAccountRepository());
        }
        public static ICurrencyService GetCurrencyService()
        {
            return new CurrencyServiceImpl(GetCurrencyRepository());
        }
        public static ICurrencyConversionService GetCurrencyConversionService()
        {
            return new CurrencyConversionServiceImpl(GetCurrencyConversionRepository());
        }
        public static ITransactionService GetTransactionService()
        {
            return new TransactionServiceImpl(GetAccountService(), GetPaymentService(), GetCurrencyConversionService());
        }
        public static AccountUtils GetAccountServiceView()
        {
            return new AccountUtils(GetAccountService(), GetPasswordService(), GetCurrencyService(), GetCurrencyConversionService());
        }
    }
}
