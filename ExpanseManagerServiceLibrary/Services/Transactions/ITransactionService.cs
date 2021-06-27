using ExpanseManagerDBLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpanseManagerServiceLibrary.Services.Transactions
{
    public interface ITransactionService
    {
        Task<bool> TransferBallance(Account from, Account to, decimal amount);
    }
}
