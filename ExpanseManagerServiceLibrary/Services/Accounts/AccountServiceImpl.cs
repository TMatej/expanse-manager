using ExpanseManagerDBLibrary.Models;
using ExpanseManagerDBLibrary.Repositories.Accounts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpanseManagerServiceLibrary.Services.Accounts
{
    public class AccountServiceImpl : IAccountService
    {
        public IAccountRepository AccountRepository { get; }

        public AccountServiceImpl(IAccountRepository accountRepository)
        {
            AccountRepository = accountRepository;
        }

        public async Task<Account> GetAccountByUserNameAsync(string username)
        {
            return await AccountRepository.GetAccountByUsernameAsync(username);
        }

        public async Task<Account> CreateAccountAsync(Account account)
        {
            return await AccountRepository.StoreAccountAsync(account);
        }

        public async Task<bool> ValidateUsernameAsync(string username)
        {
            if (username == null || username.Length == 0)
            {
                return false;
            }

            var result = await AccountRepository.GetAccountByUsernameAsync(username);

            return result == null;
        }

        public async Task<List<Account>> GetAllAccountsAsync()
        {
            return await AccountRepository.GetAllAccountsAsync();
        }

        public async Task<bool> UpdateAccountAsync(Account account)
        {
            return await AccountRepository.UpdateAccountAsync(account);
        }

        public async Task<bool> DeleteAccountAsync(long id)
        {
            return await AccountRepository.DeleteAccountAsync(id);
        }
    }
}
