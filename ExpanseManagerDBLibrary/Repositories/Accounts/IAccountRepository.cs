using ExpanseManagerDBLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpanseManagerDBLibrary.Repositories.Accounts
{
    public interface IAccountRepository
    {
        Task<List<AccountModel>> GetAllAccountsAsync();
        Task<AccountModel> GetAccountByIdAsync(long id);
        Task<AccountModel> GetAccountByUsernameAsync(string username);
        Task<AccountModel> StoreAccountAsync(AccountModel account);
        Task<bool> UpdateAccountAsync(AccountModel account);
        Task<bool> DeleteAccountAsync(long id);
    }
}
