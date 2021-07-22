using ExpanseManagerDBLibrary.Models;
using ExpanseManagerDBLibrary.Repositories.Currencies;
using System.Collections.Generic;
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

        public async Task<List<CurrencyModel>> GetAllCurrenciesAsync()
        {
            return await CurrencyRepository.GetAllCurrenciesAsync();
        }
    }
}
