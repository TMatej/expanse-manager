using ExpanseManagerDBLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpanseManagerDBLibrary.Repositories.CurrencyConversions
{
    public interface ICurrencyConversionRepository
    {
        Task<List<CurrencyConversion>> GetAllCurrencyConversionsAsync();
        Task<CurrencyConversion> GetCurrencyConversionByIdAsync(long id);
        Task<CurrencyConversion> GetCurrencyConversionByCurrenciesIdAsync(long currencyFromId, long currencyToId);
        Task<CurrencyConversion> StoreCurrencyConversionAsync(CurrencyConversion conversion);
        Task<bool> UpdateCurrencyConversionAsync(CurrencyConversion conversion);
        Task<bool> DeleteCurrencyConversionAsync(long id);
    }
}
