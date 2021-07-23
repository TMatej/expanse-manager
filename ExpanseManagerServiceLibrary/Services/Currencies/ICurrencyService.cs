using ExpanseManagerDBLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpanseManagerServiceLibrary.Services.Currencies
{
    public interface ICurrencyService
    {
        Task<List<CurrencyModel>> GetAllCurrenciesAsync();
        Task<CurrencyModel> CreateCurrency(CurrencyModel currency);
    }
}
