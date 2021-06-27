using Dapper;
using ExpanseManagerDBLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpanseManagerDBLibrary.Repositories.Currencies
{
    public class CurrencyRepositoryImpl : ICurrencyRepository
    {
        public async Task<bool> DeleteCurrencyAsync(long id)
        {
            var sql = @"DELETE FROM currency
                        WHERE Id = @Id;";

            using IDbConnection cnn = new SQLiteConnection(SqliteDataAccess.LoadConnectionString());

            var result = await cnn.ExecuteAsync(sql, new { Id = id });

            return result != 0;
        }

        public async Task<List<Currency>> GetAllCurrenciesAsync()
        {
            var sql = @"SELECT * 
                        FROM currency;";

            using IDbConnection cnn = new SQLiteConnection(SqliteDataAccess.LoadConnectionString());

            var result = await cnn.QueryAsync<Currency>(sql);
            return result.AsList();
        }

        /*May throw not found exception*/
        public async Task<Currency> GetCurrencyByIdAsync(long id)
        {
            var sql = @"SELECT * 
                        FROM currency 
                        WHERE currency.Id = @CurrencyId;";
            var parameters = new { CurrencyId = id };

            using IDbConnection cnn = new SQLiteConnection(SqliteDataAccess.LoadConnectionString());

            return await cnn.QuerySingleOrDefaultAsync<Currency>(sql, parameters);
        }

        /*May throw not found exception*/
        public async Task<Currency> GetCurrencyByShortNameAsync(string shortName)
        {
            var sql = @"SELECT * 
                        FROM currency 
                        WHERE currency.ShortName = @ShortName;";

            var parameters = new { ShortName = shortName };

            using IDbConnection cnn = new SQLiteConnection(SqliteDataAccess.LoadConnectionString());

            return await cnn.QuerySingleOrDefaultAsync<Currency>(sql, parameters);
        }

        /*May throw validation exception*/
        public async Task<Currency> StoreCurrencyAsync(Currency currency)
        {
            var sql = @"INSERT INTO currency(Name, ShortName) 
                            VALUES(@Name, @ShortName); 
                        SELECT last_insert_rowid();";

            using IDbConnection cnn = new SQLiteConnection(SqliteDataAccess.LoadConnectionString());

            var result = await cnn.QueryAsync<long>(sql, currency);
            currency.Id = result.Single();
            
            return currency;
        }

        public async Task<bool> UpdateCurrencyAsync(Currency currency)
        {
            var sql = @"UPDATE currency 
                        SET Name = @Name,
                            ShortName = @ShortName
                        WHERE currency.Id = @Id;";

            using IDbConnection cnn = new SQLiteConnection(SqliteDataAccess.LoadConnectionString());

            var result = await cnn.ExecuteAsync(sql, currency);

            return result != 0;
        }
    }
}
