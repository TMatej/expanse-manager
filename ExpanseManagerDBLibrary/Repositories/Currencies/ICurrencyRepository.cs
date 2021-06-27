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
        Task<List<Currency>> GetAllCurrenciesAsync();
        Task<Currency> GetCurrencyByIdAsync(long id);
        Task<Currency> GetCurrencyByShortNameAsync(string shortName);
        Task<Currency> StoreCurrencyAsync(Currency currency);
        Task<bool> UpdateCurrencyAsync(Currency currency);
        Task<bool> DeleteCurrencyAsync(long id);
    }
}
