using ExpanseManagerDBLibrary.Models;
using ExpanseManagerDBLibrary.Repositories.Currencies;
using ExpanseManagerServiceLibrary.Exceptions;
using System.Collections.Generic;
using System.Data.Common;
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

        public async Task<CurrencyModel> CreateCurrency(CurrencyModel currency)
        {
            try
            {
                return await CurrencyRepository.StoreCurrencyAsync(currency);
            }
            catch (DbException ex)
            { 
                throw new CouldntCreateCurrencyException(ex);
            }
        }
    }
}
