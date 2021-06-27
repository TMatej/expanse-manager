using ExpanseManagerDBLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpanseManagerServiceLibrary.Services.CurrencyConversions
{
    public interface ICurrencyConversionService
    {

        Task<decimal> GetExchangeRate(Currency source, Currency targer);
        Task<decimal> Convert(Currency source, Currency target, decimal amount);
    }
}
