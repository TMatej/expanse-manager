using Newtonsoft.Json;
using System;

namespace ExpanseManagerDBLibrary.Models
{
    public class CurrencyConversionModel : IJsonConvertable
    {
        public long Id { get; set; }
        public CurrencyModel CurrencyFrom { get; set; }
        public CurrencyModel CurrencyTo { get; set; }
        public decimal Rate { get; set; }
        public DateTime LastTimeUpdated { get; set; }

        public CurrencyConversionModel() { }

        public CurrencyConversionModel(CurrencyModel currencyFrom, CurrencyModel currencyTo, decimal rate, DateTime lastTimeUpdated)
        {
            CurrencyFrom = currencyFrom;
            CurrencyTo = currencyTo;
            Rate = rate;
            LastTimeUpdated = lastTimeUpdated;
        }

        public string ToJSON(Formatting formatting = Formatting.Indented)
        {
            return JsonConvert.SerializeObject(this, formatting);
        }
    }
}
