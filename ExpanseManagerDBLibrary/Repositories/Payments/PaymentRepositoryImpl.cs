using Dapper;
using ExpanseManagerDBLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpanseManagerDBLibrary.Repositories.Payments
{
    public class PaymentRepositoryImpl : IPaymentRepository
    {
        public async Task<bool> DeletePaymentAsync(long id)
        {
            var sql = @"DELETE FROM payment
                        WHERE payment.Id = @Id;";

            var parameters = new { Id = id };

            using IDbConnection cnn = new SQLiteConnection(SqliteDataAccess.LoadConnectionString());
            var result = await cnn.ExecuteAsync(sql, parameters);

            return result != 0;
        }

        public async Task<List<PaymentModel>> GetAllPaymentsAsync(int? limit = null)
        {
            var sql = @"SELECT 
                          payment.Id,
                          payment.SenderAmount,
                          payment.ReceiverAmount,
                          payment.TransferDate,
                          payment.Sender,
                          sender.Id,
                          sender.Name,
                          sender.UserName,
                          sender.PasswordHash,
                          sender.Ballance,
                          sender.Gender,
                          sender.LastTimeLogedIn,
                          sender.Currency,
                          innerSenderCurrency.Id,
                          innerSenderCurrency.Name,
                          innerSenderCurrency.ShortName,
                          payment.SenderCurrency,
                          senderCurrency.Id,
                          senderCurrency.Name,
                          senderCurrency.ShortName,
                          payment.Receiver,
                          receiver.Id,
                          receiver.Name,
                          receiver.UserName,
                          receiver.PasswordHash,
                          receiver.Ballance,
                          receiver.Gender,
                          receiver.LastTimeLogedIn,
                          receiver.Currency,
                          innerReceiverCurrency.Id,
                          innerReceiverCurrency.Name,
                          innerReceiverCurrency.ShortName,
                          payment.ReceiverCurrency,
                          receiverCurrency.Id,
                          receiverCurrency.Name, 
                          receiverCurrency.ShortName
                        FROM payment
                        LEFT JOIN account AS sender
                          ON payment.Sender = sender.Id
                        LEFT JOIN currency AS innerSenderCurrency
                          ON sender.Id = innerSenderCurrency.Id  
                        LEFT JOIN currency AS SenderCurrency
                          ON payment.SenderCurrency = SenderCurrency.Id
                        LEFT JOIN account AS receiver
                          ON payment.Receiver = receiver.Id
                        LEFT JOIN currency AS innerReceiverCurrency
                          ON receiver.Id = innerReceiverCurrency.Id 
                        LEFT JOIN currency AS ReceiverCurrency
                          ON payment.ReceiverCurrency = ReceiverCurrency.Id";

            if (limit != null && limit > 0)
            {
                sql += $" LIMIT {limit}";
            }

            sql += ";";

            var result = await StartGetQuery(sql, new { });
            return result.AsList();
        }

        public async Task<PaymentModel> GetPaymentByIdAsync(long id)
        {
            var sql = @"SELECT 
                          payment.Id,
                          payment.SenderAmount,
                          payment.ReceiverAmount,
                          payment.TransferDate,
                          payment.Sender,
                          sender.Id,
                          sender.Name,
                          sender.UserName,
                          sender.PasswordHash,
                          sender.Ballance,
                          sender.Gender,
                          sender.LastTimeLogedIn,
                          sender.Currency,
                          innerSenderCurrency.Id,
                          innerSenderCurrency.Name,
                          innerSenderCurrency.ShortName,
                          payment.SenderCurrency,
                          senderCurrency.Id,
                          senderCurrency.Name,
                          senderCurrency.ShortName,
                          payment.Receiver,
                          receiver.Id,
                          receiver.Name,
                          receiver.UserName,
                          receiver.PasswordHash,
                          receiver.Ballance,
                          receiver.Gender,
                          receiver.LastTimeLogedIn,
                          receiver.Currency,
                          innerReceiverCurrency.Id,
                          innerReceiverCurrency.Name,
                          innerReceiverCurrency.ShortName,
                          payment.ReceiverCurrency,
                          receiverCurrency.Id,
                          receiverCurrency.Name, 
                          receiverCurrency.ShortName
                        FROM (SELECT * FROM payment WHERE payment.Id = @Id) AS payment
                        LEFT JOIN account AS sender
                          ON payment.Sender = sender.Id
                        LEFT JOIN currency AS innerSenderCurrency
                          ON sender.Id = innerSenderCurrency.Id  
                        LEFT JOIN currency AS SenderCurrency
                          ON payment.SenderCurrency = SenderCurrency.Id
                        LEFT JOIN account AS receiver
                          ON payment.Receiver = receiver.Id
                        LEFT JOIN currency AS innerReceiverCurrency
                          ON receiver.Id = innerReceiverCurrency.Id 
                        LEFT JOIN currency AS ReceiverCurrency
                          ON payment.ReceiverCurrency = ReceiverCurrency.Id;";

            var parameters = new { Id = id };

            var result = await StartGetQuery(sql, parameters);

            return result.SingleOrDefault();
        }

        public async Task<List<PaymentModel>> GetPaymentsByReceiverIdAsync(long id, int? limit = null)
        {
            var sql = @"SELECT 
                          payment.Id,
                          payment.SenderAmount,
                          payment.ReceiverAmount,
                          payment.TransferDate,
                          payment.Sender,
                          sender.Id,
                          sender.Name,
                          sender.UserName,
                          sender.PasswordHash,
                          sender.Ballance,
                          sender.Gender,
                          sender.LastTimeLogedIn,
                          sender.Currency,
                          innerSenderCurrency.Id,
                          innerSenderCurrency.Name,
                          innerSenderCurrency.ShortName,
                          payment.SenderCurrency,
                          senderCurrency.Id,
                          senderCurrency.Name,
                          senderCurrency.ShortName,
                          payment.Receiver,
                          receiver.Id,
                          receiver.Name,
                          receiver.UserName,
                          receiver.PasswordHash,
                          receiver.Ballance,
                          receiver.Gender,
                          receiver.LastTimeLogedIn,
                          receiver.Currency,
                          innerReceiverCurrency.Id,
                          innerReceiverCurrency.Name,
                          innerReceiverCurrency.ShortName,
                          payment.ReceiverCurrency,
                          receiverCurrency.Id,
                          receiverCurrency.Name, 
                          receiverCurrency.ShortName
                        FROM (SELECT * FROM payment WHERE payment.Receiver = @ReceiverId) AS payment
                        LEFT JOIN account AS sender
                          ON payment.Sender = sender.Id
                        LEFT JOIN currency AS innerSenderCurrency
                          ON sender.Id = innerSenderCurrency.Id  
                        LEFT JOIN currency AS SenderCurrency
                          ON payment.SenderCurrency = SenderCurrency.Id
                        LEFT JOIN account AS receiver
                          ON payment.Receiver = receiver.Id
                        LEFT JOIN currency AS innerReceiverCurrency
                          ON receiver.Id = innerReceiverCurrency.Id 
                        LEFT JOIN currency AS ReceiverCurrency
                          ON payment.ReceiverCurrency = ReceiverCurrency.Id";

            if (limit != null && limit > 0)
            {
                sql += $" LIMIT {limit}";
            }

            sql += ";";

            var parameters = new { ReceiverId = id };

            var result = await StartGetQuery(sql, parameters);

            return result.AsList();
        }

        public async Task<List<PaymentModel>> GetPaymentsBySenderIdAsync(long id, int? limit = null)
        {
            var sql = @"SELECT 
                          payment.Id,
                          payment.SenderAmount,
                          payment.ReceiverAmount,
                          payment.TransferDate,
                          payment.Sender,
                          sender.Id,
                          sender.Name,
                          sender.UserName,
                          sender.PasswordHash,
                          sender.Ballance,
                          sender.Gender,
                          sender.LastTimeLogedIn,
                          sender.Currency,
                          innerSenderCurrency.Id,
                          innerSenderCurrency.Name,
                          innerSenderCurrency.ShortName,
                          payment.SenderCurrency,
                          senderCurrency.Id,
                          senderCurrency.Name,
                          senderCurrency.ShortName,
                          payment.Receiver,
                          receiver.Id,
                          receiver.Name,
                          receiver.UserName,
                          receiver.PasswordHash,
                          receiver.Ballance,
                          receiver.Gender,
                          receiver.LastTimeLogedIn,
                          receiver.Currency,
                          innerReceiverCurrency.Id,
                          innerReceiverCurrency.Name,
                          innerReceiverCurrency.ShortName,
                          payment.ReceiverCurrency,
                          receiverCurrency.Id,
                          receiverCurrency.Name, 
                          receiverCurrency.ShortName
                        FROM (SELECT * FROM payment WHERE payment.Sender = @SenderId) AS payment
                        LEFT JOIN account AS sender
                          ON payment.Sender = sender.Id
                        LEFT JOIN currency AS innerSenderCurrency
                          ON sender.Id = innerSenderCurrency.Id  
                        LEFT JOIN currency AS SenderCurrency
                          ON payment.SenderCurrency = SenderCurrency.Id
                        LEFT JOIN account AS receiver
                          ON payment.Receiver = receiver.Id
                        LEFT JOIN currency AS innerReceiverCurrency
                          ON receiver.Id = innerReceiverCurrency.Id 
                        LEFT JOIN currency AS ReceiverCurrency
                          ON payment.ReceiverCurrency = ReceiverCurrency.Id";

            if (limit != null && limit > 0)
            {
                sql += $" LIMIT {limit}";
            }

            sql += ";";

            var parameters = new { SenderId = id };

            var result = await StartGetQuery(sql, parameters);

            return result.AsList();
        }

        public async Task<PaymentModel> StorePaymentAsync(PaymentModel payment)
        {
            var sql = @"INSERT INTO payment(Sender, SenderCurrency, SenderAmount, Receiver, ReceiverCurrency, ReceiverAmount, TransferDate)
                        VALUES(@SenderId, @SenderCurrencyId, @SenderAmount, @ReceiverId, @ReceiverCurrencyId, @ReceiverAmount, @TransferDate);
                        SELECT last_insert_rowid();";

            var parameters = new 
            {
                SenderId = payment.Sender.Id,
                SenderCurrencyId = payment.Sender.Currency.Id,
                payment.SenderAmount,
                ReceiverId = payment.Receiver.Id,
                ReceiverCurrencyId = payment.Receiver.Currency.Id,
                payment.ReceiverAmount,
                payment.TransferDate
            };

            using IDbConnection cnn = new SQLiteConnection(SqliteDataAccess.LoadConnectionString());
            var result = await cnn.QueryAsync<long>(sql, parameters);
            payment.Id = result.Single();

            return payment;
        }

        private async Task<IEnumerable<PaymentModel>> StartGetQuery<T>(string sql, T parameters)
        {

            using IDbConnection cnn = new SQLiteConnection(SqliteDataAccess.LoadConnectionString());

            var result = await cnn.QueryAsync<PaymentModel, AccountModel, CurrencyModel, CurrencyModel, AccountModel, CurrencyModel, CurrencyModel, PaymentModel>(sql,
                (payment, sender, innerSenderCurrency, senderCurrency, receiver, innerReceiverCurrency, receiverCurrency) =>
                {
                    sender.Currency = innerSenderCurrency;
                    payment.Sender = sender;
                    payment.SenderCurrency = senderCurrency;
                    receiver.Currency = innerReceiverCurrency;
                    payment.Receiver = receiver;
                    payment.ReceiverCurrency = receiverCurrency;
                    return payment;
                },
                splitOn: "Sender, Currency, SenderCurrency, Receiver, Currency, ReceiverCurrency",
                param: parameters);

            return result;
        }

        /* UpdatePaymentAsync - is not allowed */
    }
}
