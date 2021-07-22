﻿using ExpanseManagerDBLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpanseManagerServiceLibrary.Services.Payments
{
    public interface IPaymentService
    {
        Task<PaymentModel> CreatePayment(PaymentModel payment);
    }
}
