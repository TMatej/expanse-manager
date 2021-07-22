using ExpanseManagerDBLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpanseManagerDBLibrary.Repositories.Currencies
{
    public interface ICurrencyRepository
    {
        Task<List<CurrencyModel>> GetAllCurrenciesAsync();
        Task<CurrencyModel> GetCurrencyByIdAsync(long id);
        Task<CurrencyModel> GetCurrencyByShortNameAsync(string shortName);
        Task<CurrencyModel> StoreCurrencyAsync(CurrencyModel currency);
        Task<bool> UpdateCurrencyAsync(CurrencyModel currency);
        Task<bool> DeleteCurrencyAsync(long id);
    }
}
