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
        Task<List<Payment>> GetAllPaymentsAsync(int? limit);
        Task<Payment> GetPaymentByIdAsync(long id);
        Task<List<Payment>> GetPaymentsBySenderIdAsync(long id, int? limit);
        Task<List<Payment>> GetPaymentsByReceiverIdAsync(long id, int? limit);
        Task<Payment> StorePaymentAsync(Payment payment);
        Task<bool> DeletePaymentAsync(long id);
    }
}
