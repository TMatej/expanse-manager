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
        Task<List<AccountModel>> GetAllAccountsAsync();
        Task<AccountModel> GetAccountByUserNameAsync(string username);
        Task<AccountModel> CreateAccountAsync(AccountModel account);
        Task<bool> UpdateAccountAsync(AccountModel account);
        Task<bool> DeleteAccountAsync(long id);
    }
}
