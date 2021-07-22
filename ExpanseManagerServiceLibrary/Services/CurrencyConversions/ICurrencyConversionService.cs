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
        Task<decimal> Convert(CurrencyModel source, CurrencyModel target, decimal amount);
        Task<decimal> GetExchangeRate(CurrencyModel source, CurrencyModel targer);
        Task<CurrencyConversionModel> CreateCurrencyConversion(CurrencyConversionModel currencyConversion);
        Task<bool> UpdateCurrencyConversion(CurrencyConversionModel currencyConversion);
        Task<bool> DeleteCurrencyConversion(long id);
    }
}
