using ExpanseManagerDBLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpanseManagerServiceLibrary.Services.CurrencyConversions
{
    public class CurrencyConversionServiceImpl : ICurrencyConversionService
    {



        public Task<decimal> Convert(Currency source, Currency target, decimal amount)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> GetExchangeRate(Currency source, Currency targer)
        {
            throw new NotImplementedException();
        }
    }
}
