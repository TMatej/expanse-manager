using ExpanseManagerDBLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpanseManagerDBLibrary.Repositories.Payments
{
    public interface IPaymentRepository
    {
        Task<List<PaymentModel>> GetAllPaymentsAsync(int? limit);
        Task<PaymentModel> GetPaymentByIdAsync(long id);
        Task<List<PaymentModel>> GetPaymentsBySenderIdAsync(long id, int? limit);
        Task<List<PaymentModel>> GetPaymentsByReceiverIdAsync(long id, int? limit);
        Task<PaymentModel> StorePaymentAsync(PaymentModel payment);
        Task<bool> DeletePaymentAsync(long id);
    }
}
