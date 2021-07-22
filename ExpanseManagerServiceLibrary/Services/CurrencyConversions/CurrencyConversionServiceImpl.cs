using ExpanseManagerDBLibrary.Models;
using ExpanseManagerDBLibrary.Repositories.CurrencyConversions;
using ExpanseManagerServiceLibrary.Exceptions;
using System;
using System.Threading.Tasks;

namespace ExpanseManagerServiceLibrary.Services.CurrencyConversions
{
    public class CurrencyConversionServiceImpl : ICurrencyConversionService
    {

        public ICurrencyConversionRepository CurrencyConversionRepository { get; }

        public CurrencyConversionServiceImpl(ICurrencyConversionRepository currencyConversionRepository)
        {
            CurrencyConversionRepository = currencyConversionRepository;
        }

        public async Task<decimal> Convert(CurrencyModel source, CurrencyModel target, decimal amount)
        {
            var rate = await GetExchangeRate(source, target);
            
            return decimal.Multiply(rate, amount);
        }

        public async Task<decimal> GetExchangeRate(CurrencyModel source, CurrencyModel target)
        {
            if (source == null)
            {
                throw new ArgumentNullException("Source currency is null!");
            }

            if (target == null)
            {
                throw new ArgumentNullException("Target currency is null!");
            }

            var currencyConversion = await CurrencyConversionRepository.GetCurrencyConversionByCurrenciesIdAsync(source.Id, target.Id);

            if (currencyConversion == null)
            {
                throw new UnknownExchangeRateException("Currency conversion unknown!");
            }

            return currencyConversion.Rate;
        }

        public async Task<CurrencyConversionModel> CreateCurrencyConversion(CurrencyConversionModel currencyConversion)
        {
            return await CurrencyConversionRepository.StoreCurrencyConversionAsync(currencyConversion);
        }

        public async Task<bool> UpdateCurrencyConversion(CurrencyConversionModel currencyConversion)
        {
            return await CurrencyConversionRepository.UpdateCurrencyConversionAsync(currencyConversion);
        }

        public async Task<bool> DeleteCurrencyConversion(long id)
        {
            return await CurrencyConversionRepository.DeleteCurrencyConversionAsync(id);
        }
    }
}
