using ExpanseManagerDBLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpanseManagerServiceLibrary.Services.Accounts
{
    public interface IAccountService
    {
        Task<bool> ValidateUsernameAsync(string username);
        Task<List<Account>> GetAllAccountsAsync();
        Task<Account> GetAccountByUserNameAsync(string username);
        Task<Account> CreateAccountAsync(Account account);
        Task<bool> UpdateAccountAsync(Account account);
        Task<bool> DeleteAccountAsync(long id);
    }
}
