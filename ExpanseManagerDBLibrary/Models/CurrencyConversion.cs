using Newtonsoft.Json;
using System;

namespace ExpanseManagerDBLibrary.Models
{
    public class CurrencyConversion : IJsonConvertable
    {
        public long Id { get; set; }
        public Currency CurrencyFrom { get; set; }
        public Currency CurrencyTo { get; set; }
        public decimal Rate { get; set; }
        public DateTime LastTimeUpdated { get; set; }

        public CurrencyConversion() { }

        public CurrencyConversion(Currency currencyFrom, Currency currencyTo, decimal rate, DateTime lastTimeUpdated)
        {
            CurrencyFrom = currencyFrom;
            CurrencyTo = currencyTo;
            Rate = rate;
            LastTimeUpdated = lastTimeUpdated;
        }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
