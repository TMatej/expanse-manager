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
        Task<List<CurrencyConversionModel>> GetAllCurrencyConversionsAsync();
        Task<CurrencyConversionModel> GetCurrencyConversionByIdAsync(long id);
        Task<CurrencyConversionModel> GetCurrencyConversionByCurrenciesIdAsync(long currencyFromId, long currencyToId);
        Task<CurrencyConversionModel> StoreCurrencyConversionAsync(CurrencyConversionModel conversion);
        Task<bool> UpdateCurrencyConversionAsync(CurrencyConversionModel conversion);
        Task<bool> DeleteCurrencyConversionAsync(long id);
    }
}
