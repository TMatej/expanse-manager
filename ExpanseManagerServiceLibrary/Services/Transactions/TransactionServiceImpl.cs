using ExpanseManagerDBLibrary.Models;
using ExpanseManagerServiceLibrary.Exceptions;
using ExpanseManagerServiceLibrary.Services.Accounts;
using ExpanseManagerServiceLibrary.Services.CurrencyConversions;
using ExpanseManagerServiceLibrary.Services.Payments;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace ExpanseManagerServiceLibrary.Services.Transactions
{
    public class TransactionServiceImpl : ITransactionService
    {
        public IAccountService AccountService { get; }
        public IPaymentService PaymentService { get; }
        public ICurrencyConversionService CurrencyConversionService { get; }

        public TransactionServiceImpl(IAccountService accountService, IPaymentService paymentService, ICurrencyConversionService currencyConversionService)
        {
            AccountService = accountService;
            PaymentService = paymentService;
            CurrencyConversionService = currencyConversionService;
        }

        
        public async Task<decimal> TransferBallance(AccountModel from, AccountModel to, decimal amount)
        {
            var amountToTransfer = amount;

            if (!from.Currency.Equals(to.Currency))
            {
                // may throw UnknownExchangeRateException
                amountToTransfer = await CurrencyConversionService.Convert(from.Currency, to.Currency, amount);
            }

            if (from.Ballance < amountToTransfer)
            {
                throw new InsufficientBallanceException($"Account {from.UserName} doesn't have sufficient financial resources.");
            }

            from.Ballance = decimal.Subtract(from.Ballance, amount);
            to.Ballance = decimal.Add(to.Ballance, amountToTransfer);
            var payment = new PaymentModel(from, from.Currency, amount, to, to.Currency, amountToTransfer, DateTime.Now);

            /*This has to be one transaction!*/
            using (var transaction = new TransactionScope())
            {
                Task.WaitAll(PaymentService.CreatePayment(payment),
                AccountService.UpdateAccountAsync(from),
                AccountService.UpdateAccountAsync(to));

                transaction.Complete();
            }
            
            return amountToTransfer;
        }
    }
}
