using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace ExpanseManagerDBLibrary.Models
{
    public class CurrencyModel : IJsonConvertable
    {
        [Key]
        public long Id { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }

        public CurrencyModel() { }

        public CurrencyModel(string shortName, string name) 
        {
            ShortName = shortName;
            Name = name;
        }

        public string ToJSON(Formatting formatting = Formatting.Indented)
        {
            return JsonConvert.SerializeObject(this, formatting);
        }

        public override bool Equals(object obj)
        {
            return obj is CurrencyModel currency &&
                   ShortName == currency.ShortName &&
                   Name == currency.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ShortName, Name);
        }
    }
}
