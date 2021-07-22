using Dapper;
using ExpanseManagerDBLibrary.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace ExpanseManagerDBLibrary.Repositories.CurrencyConversions
{
    public class CurrencyConversionRepositoryImpl : ICurrencyConversionRepository
    {
        public async Task<bool> DeleteCurrencyConversionAsync(long id)
        {
            var sql = @"DELETE FROM currency_conversion
                        WHERE Id = @Id;";

            using IDbConnection cnn = new SQLiteConnection(SqliteDataAccess.LoadConnectionString());

            var result = await cnn.ExecuteAsync(sql, new { Id = id });

            return result != 0;
        }

        public async Task<List<CurrencyConversionModel>> GetAllCurrencyConversionsAsync()
        {
            var sql = @"SELECT 
                          conversion.Id,
                          conversion.Rate,
                          conversion.LastTimeUpdated,
                          conversion.CurrencyFrom,
                          currencyFrom.Id,
                          currencyFrom.Name,
                          currencyFrom.ShortName,
                          conversion.CurrencyTo,
                          currencyTo.Id,
                          currencyTo.Name,
                          currencyTo.ShortName
                        FROM currency_conversion AS conversion
                        LEFT JOIN currency AS currencyFrom
                          ON conversion.CurrencyFrom = currencyFrom.Id
                        LEFT JOIN currency AS currencyTo
                          ON conversion.CurrencyTo = currencyTo.Id;";

            using IDbConnection cnn = new SQLiteConnection(SqliteDataAccess.LoadConnectionString());
            var result = await cnn.QueryAsync<CurrencyConversionModel, CurrencyModel, CurrencyModel, CurrencyConversionModel>(sql,
                (conversion, currencyFrom, currencyTo) =>
                {
                    conversion.CurrencyFrom = currencyFrom;
                    conversion.CurrencyTo = currencyTo;
                    return conversion;
                },
                splitOn: "CurrencyFrom, CurrencyTo");

            return result.AsList();
        }

        public async Task<CurrencyConversionModel> GetCurrencyConversionByCurrenciesIdAsync(long currencyFromId, long currencyToId)
        {
            /*split in unique queries or start it in one query as follows?*/
            var sql = @"SELECT 
                          conversion.Id,
                          conversion.Rate,
                          conversion.LastTimeUpdated,
                          conversion.CurrencyFrom,
                          currencyFrom.Id,
                          currencyFrom.Name,
                          currencyFrom.ShortName,
                          conversion.CurrencyTo,
                          currencyTo.Id,
                          currencyTo.Name,
                          currencyTo.ShortName
                        FROM (SELECT * 
                              FROM currency_conversion 
                              WHERE currency_conversion.CurrencyFrom = @FromId
                                AND currency_conversion.CurrencyTo = @ToId) AS conversion
                        LEFT JOIN currency AS currencyFrom
                          ON conversion.CurrencyFrom = currencyFrom.Id
                        LEFT JOIN currency AS currencyTo
                          ON conversion.CurrencyTo = currencyTo.Id;";

            var parameters = new { FromId = currencyFromId, ToId = currencyToId };

            using IDbConnection cnn = new SQLiteConnection(SqliteDataAccess.LoadConnectionString());
            var result = await cnn.QueryAsync<CurrencyConversionModel, CurrencyModel, CurrencyModel, CurrencyConversionModel>(sql,
                (conversion, currencyFrom, currencyTo) =>
                {
                    conversion.CurrencyFrom = currencyFrom;
                    conversion.CurrencyTo = currencyTo;
                    return conversion;
                },
                splitOn: "CurrencyFrom, CurrencyTo",
                param: parameters);

            return result.SingleOrDefault();
        }

        public async Task<CurrencyConversionModel> GetCurrencyConversionByIdAsync(long id)
        {
            var sql = @"SELECT 
                          conversion.Id,
                          conversion.Rate,
                          conversion.LastTimeUpdated,
                          conversion.CurrencyFrom,
                          currencyFrom.Id,
                          currencyFrom.Name,
                          currencyFrom.ShortName,
                          conversion.CurrencyTo,
                          currencyTo.Id,
                          currencyTo.Name,
                          currencyTo.ShortName
                        FROM (SELECT * 
                              FROM currency_conversion 
                              WHERE currency_conversion.Id = @Id) AS conversion
                        LEFT JOIN currency AS currencyFrom
                          ON conversion.CurrencyFrom = currencyFrom.Id
                        LEFT JOIN currency AS currencyTo
                          ON conversion.CurrencyTo = currencyTo.Id;";

            var parameters = new { Id = id };

            using IDbConnection cnn = new SQLiteConnection(SqliteDataAccess.LoadConnectionString());
            var result = await cnn.QueryAsync<CurrencyConversionModel, CurrencyModel, CurrencyModel, CurrencyConversionModel>(sql,
                (conversion, currencyFrom, currencyTo) =>
                {
                    conversion.CurrencyFrom = currencyFrom;
                    conversion.CurrencyTo = currencyTo;
                    return conversion;
                },
                splitOn: "CurrencyFrom, CurrencyTo",
                param: parameters);

            return result.SingleOrDefault();
        }

        public async Task<CurrencyConversionModel> StoreCurrencyConversionAsync(CurrencyConversionModel conversion)
        {
            var sql = @"INSERT INTO currency_conversion(CurrencyFrom, CurrencyTo, Rate, LastTimeUpdated)
                        VALUES(@CurrencyFromId, @CurrencyToId, @Rate, @LastTimeUpdated);
                        SELECT last_insert_rowid();";

            var parameters = new
            {
                CurrencyFromId = conversion.CurrencyFrom.Id,
                CurrencyToId = conversion.CurrencyTo.Id,
                conversion.Rate,
                conversion.LastTimeUpdated
            };

            using IDbConnection cnn = new SQLiteConnection(SqliteDataAccess.LoadConnectionString());
            var result = await cnn.QueryAsync<long>(sql, parameters);
            conversion.Id = result.Single();

            return conversion;
        }

        public async Task<bool> UpdateCurrencyConversionAsync(CurrencyConversionModel conversion)
        {
            var sql = @"UPDATE currency_conversion 
                        SET CurrencyFrom = @CurrencyFrom,
                            CurrencyTo = @CurrencyTo,
                            Rate = @Rate,
                            LastTimeUpdated = @LastTimeUpdated
                        WHERE currency_conversion.Id = @Id;";

            using IDbConnection cnn = new SQLiteConnection(SqliteDataAccess.LoadConnectionString());

            var result = await cnn.ExecuteAsync(sql, conversion);

            return result != 0;
        }
    }
}
