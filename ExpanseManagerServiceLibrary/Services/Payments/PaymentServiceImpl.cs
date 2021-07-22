using ExpanseManagerDBLibrary.Models;
using ExpanseManagerDBLibrary.Repositories.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpanseManagerServiceLibrary.Services.Payments
{
    public class PaymentServiceImpl : IPaymentService
    {
        public IPaymentRepository PaymentRepository { get; }

        public PaymentServiceImpl(IPaymentRepository paymentRepository)
        {
            PaymentRepository = paymentRepository;
        }

        public async Task<PaymentModel> CreatePayment(PaymentModel payment)
        {
            return await PaymentRepository.StorePaymentAsync(payment);
        }
    }
}
