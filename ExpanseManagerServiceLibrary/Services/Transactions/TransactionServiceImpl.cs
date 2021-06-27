using ExpanseManagerDBLibrary.Models;
using ExpanseManagerServiceLibrary.Services.Accounts;
using ExpanseManagerServiceLibrary.Services.CurrencyConversions;
using ExpanseManagerServiceLibrary.Services.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpanseManagerServiceLibrary.Services.Transactions
{
    public class TransactionServiceImpl : ITransactionService
    {
        public IAccountService AccountService { get; }
        public IPaymentService PaymentService { get; }
        public ICurrencyConversionService CurrencyConversionService { get; }
        

        public Task<bool> TransferBallance(Account from, Account to, decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}
