using ExpanseManagerDBLibrary.Models;
using ExpanseManagerDBLibrary.Repositories.Currencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpanseManagerServiceLibrary.Services.Currencies
{
    public class CurrencyServiceImpl : ICurrencyService
    {
        public ICurrencyRepository CurrencyRepository { get; }

        public CurrencyServiceImpl(ICurrencyRepository currencyRepository)
        {
            CurrencyRepository = currencyRepository;
        }

        public async Task<List<Currency>> GetAllCurrenciesAsync()
        {
            return await CurrencyRepository.GetAllCurrenciesAsync();
        }
    }
}
