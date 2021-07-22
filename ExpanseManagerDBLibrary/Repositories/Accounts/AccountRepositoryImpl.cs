using Dapper;
using ExpanseManagerDBLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpanseManagerDBLibrary.Repositories.Accounts
{
    public class AccountRepositoryImpl : IAccountRepository
    {
        public async Task<bool> DeleteAccountAsync(long id)
        {
            var sql = @"DELETE FROM account
                        WHERE Id = @Id;";

            using IDbConnection cnn = new SQLiteConnection(SqliteDataAccess.LoadConnectionString());

            Console.WriteLine($"Deleting account with id = {id}");
            var result = await cnn.ExecuteAsync(sql, new { Id = id});

            return result != 0;
        }

        public async Task<AccountModel> GetAccountByIdAsync(long id)
        {
            var sql = @"SELECT * 
                        FROM (SELECT * 
                              FROM account 
                              WHERE account.Id = @Id) AS a
                        LEFT JOIN currency as c
                            ON a.Currency = c.Id;";

            var parameters = new { Id = id };

            using IDbConnection cnn = new SQLiteConnection(SqliteDataAccess.LoadConnectionString());
            var result = await cnn.QueryAsync<AccountModel, CurrencyModel, AccountModel>(sql,
                (account, currency) =>
                {
                    account.Currency = currency;
                    return account;
                },
                splitOn: "Currency",
                param: parameters);

            return result.SingleOrDefault();
        }

        public async Task<AccountModel> GetAccountByUsernameAsync(string username)
        {
            var sql = @"SELECT * 
                        FROM (SELECT * 
                              FROM account 
                              WHERE account.UserName = @UserName) AS a
                        LEFT JOIN currency as c
                            ON a.Currency = c.Id;";

            var parameters = new { UserName = username };

            using IDbConnection cnn = new SQLiteConnection(SqliteDataAccess.LoadConnectionString());
            var result = await cnn.QueryAsync<AccountModel, CurrencyModel, AccountModel>(sql, 
                (account, currency) => 
                {
                    account.Currency = currency;
                    return account;
                },
                splitOn: "Currency",
                param: parameters);

            return result.SingleOrDefault();
        }

        public async Task<List<AccountModel>> GetAllAccountsAsync()
        {
            var sql = @"SELECT * 
                        FROM account AS a
                        LEFT JOIN currency AS c
                            ON a.Currency = c.Id;";

            using IDbConnection cnn = new SQLiteConnection(SqliteDataAccess.LoadConnectionString());

            var result = await cnn.QueryAsync<AccountModel, CurrencyModel, AccountModel>(sql,
                (account, currency) => 
                {
                    account.Currency = currency;
                    return account;
                }
                , splitOn: "Currency");
            return result.AsList();
        }

        /*May throw validation(already exists) exception*/
        public async Task<AccountModel> StoreAccountAsync(AccountModel account)
        {
            var sql = @"INSERT INTO account(Name, UserName, PasswordHash, Ballance, Gender, Currency, LastTimeLogedIn) 
                        VALUES(@Name, @UserName, @PasswordHash, @Ballance, @Gender, @CurrencyId, @LastTimeLogedIn);
                        SELECT last_insert_rowid();";

            /*consider creating custom dapper-mapper*/
            var parameter = new 
            {
                account.Name,
                account.UserName,
                account.PasswordHash,
                account.Ballance,
                account.Gender,
                CurrencyId = account.Currency.Id,
                account.LastTimeLogedIn
            };

            using IDbConnection cnn = new SQLiteConnection(SqliteDataAccess.LoadConnectionString());
            var result = await cnn.QueryAsync<long>(sql, parameter);
            account.Id = result.Single();

            return account;
        }

        public async Task<bool> UpdateAccountAsync(AccountModel account)
        {
            var sql = @"UPDATE account 
                        SET Name = @Name,
                            UserName = @UserName,
                            PasswordHash = @PasswordHash,
                            Ballance = @Ballance,
                            Gender = @Gender,
                            Currency = @CurrencyId,
                            LastTimeLogedIn = @LastTimeLogedIn
                        WHERE account.Id = @Id;";

            /*consider creating custom dapper-mapper*/
            var parameter = new 
            { 
                account.Name,
                account.UserName,
                account.PasswordHash,
                account.Ballance,
                account.Gender,
                CurrencyId = account.Currency.Id,
                account.LastTimeLogedIn,
                account.Id         
            };

            using IDbConnection cnn = new SQLiteConnection(SqliteDataAccess.LoadConnectionString());

            var result = await cnn.ExecuteAsync(sql, parameter);

            return result != 0;
        }
    }
}
