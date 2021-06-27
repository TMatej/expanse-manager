using ExpanseManagerDBLibrary.Models.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace ExpanseManagerDBLibrary.Models
{
    public class Account : IJsonConvertable
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Sex Gender { get; set; }
        public decimal Ballance { get; set; }

        public Currency Currency { get; set; }
        public DateTime? LastTimeLogedIn { get; set; }

        public Account() { }

        public Account(string userName, string passwordHash, string name, Sex gender, decimal ballance, Currency currency, DateTime? lastTimeLogedIn)
        {
            UserName = userName;
            PasswordHash = passwordHash;
            Name = name;
            Gender = gender;
            Ballance = ballance;
            Currency = currency;
            LastTimeLogedIn = lastTimeLogedIn;
        }

        public override string ToString()
        {
            var modification = LastTimeLogedIn.HasValue ? $" last time logged in on {LastTimeLogedIn.Value}" : string.Empty;
            return $"UserName: {UserName}, Name: {Name}, Gender: {Gender}, Current ballance:{Ballance}, Currency: {Currency}{modification}";
        }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        protected bool Equals(Account other)
        {
            return UserName == other.UserName;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            return obj.GetType() == this.GetType() && Equals((Account)obj);
        }

        public override int GetHashCode()
        {
            return UserName.GetHashCode();
        }
    }
}
