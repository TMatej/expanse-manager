using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ExpanseManagerDBLibrary.Models
{
    public class Currency : IJsonConvertable
    {
        [Key]
        public long Id { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }

        public Currency() { }

        public Currency(string shortName, string name) 
        {
            ShortName = shortName;
            Name = name;
        }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
