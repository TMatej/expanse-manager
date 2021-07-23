using System;
using System.Text;
using System.Text.RegularExpressions;

namespace ExpanseManagerServiceLibrary.Services.Security
{
    public class PasswordService : IPasswordService
    {
        public const string passwordValidationRegex = @"(?-i)(?=^.{8,}$)((?!.*\s)(?=.*[A-Z])(?=.*[a-z]))((?=(.*\d){1,})|(?=(.*\W){1,}))^.*$";
        private readonly string passwordCheckRegex;

        public PasswordService()
        {
            passwordCheckRegex = passwordValidationRegex;
        }

        public PasswordService(string regex)
        {
            passwordCheckRegex = regex;
        }

        public bool ValidatePassword(string password)
        {
            if (String.IsNullOrEmpty(password))
            {
                return false;
            }

            return Regex.Match(password, passwordCheckRegex).Success;
        }

        public bool VerifyPassword(string hashedPassword, string rawPassword)
        {
            var hasher = new PasswordHasher(Convert.FromBase64String(hashedPassword));
            return hasher.Verify(rawPassword);
        }

        public string EncodePassword(string password)
        {
            var hasher = new PasswordHasher(password);
            return Convert.ToBase64String(hasher.ToArray());
        }
    }
}
