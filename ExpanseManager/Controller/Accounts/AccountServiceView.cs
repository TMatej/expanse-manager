using ExpanseManager.ConsoleView;
using ExpanseManagerDBLibrary.Models;
using ExpanseManagerDBLibrary.Models.Enums;
using ExpanseManagerServiceLibrary.Services.Accounts;
using ExpanseManagerServiceLibrary.Services.Currencies;
using ExpanseManagerServiceLibrary.Services.Security;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExpanseManager.Controller.Services.Accounts
{
    public class AccountServiceView
    {

        public IAccountService AccountService { get; }
        public IPasswordService PasswordService { get; }
        public ICurrencyService CurrencyService { get; }

        public AccountServiceView(IAccountService accountService, IPasswordService passwordService, ICurrencyService currencyService)
        {
            AccountService = accountService;
            PasswordService = passwordService;
            CurrencyService = currencyService;
        }

        public Account CreateNewAccount()
        {
            Console.Clear();
            BasicOutputMessages.PrintResponseMessage("To create new account please fill in the request. All information can be changed after logging into your account.");
            Console.WriteLine();
            string username = CreateUserName();
            string password = CreatePassword();
            string name = CreateName();
            Sex gender = ChooseGender();
            Currency currency = ChooseCurrency();

            Account account = CreateAccount(username, password, name, gender, currency);

            return account;
        }

        public Account CreateRootAccount()
        {
            Console.Clear();
            BasicOutputMessages.PrintResponseMessage("To create a root account, please follow these few steps.");
            string username = "root";
            string password = CreatePassword();
            string name = "Application root account";
            Sex gender = Sex.Other;
            Currency currency = ChooseCurrency();

            Account account = CreateAccount(username, password, name, gender, currency);

            return account;
        }

        private Account CreateAccount(string username, string password, string name, Sex gender, Currency currency)
        {
            Account newAccount = new()
            {
                UserName = username,
                PasswordHash = password,
                Name = name,
                Gender = gender,
                Ballance = 0L,
                Currency = currency
            };

            newAccount = Task.Run(async () => await AccountService.CreateAccountAsync(newAccount)).Result;
            BasicOutputMessages.PrintSuccessMessage("Success! Account created!");
            Console.WriteLine();
            BasicOutputMessages.PrintResponseMessage("Sum of created account in JSON:");
            Console.WriteLine(newAccount.ToJSON());
            Console.WriteLine();
            return newAccount;
        }

        public string CreateUserName(string request = "Choose a unique username.")
        {
            string username = null;

            while (!ValidateUsername(username))
            {
                BasicOutputMessages.PrintResponseMessage(request);
                Console.Write("Username: ");
                username = Console.ReadLine().Trim();
            }

            BasicOutputMessages.PrintResponseMessage("Chosen username is: ");
            BasicOutputMessages.PrintSuccessMessage(username);

            return username;
        }

        public string CreatePassword()
        {
            string password = null;
            string secondPassword;

            while (password == null)
            {
                while (!PasswordService.ValidatePassword(password))
                {
                    Console.WriteLine();
                    BasicOutputMessages.PrintResponseMessage("Choose a strong password.");
                    Console.WriteLine("Password must have at least 8 characters with at least one Capital letter, at least one lower case letter and at least one number or special character.");
                    Console.Write("Password: ");
                    password = GetHiddenConsoleInput();
                    Console.WriteLine();
                }

                Console.WriteLine();
                BasicOutputMessages.PrintResponseMessage("Please, re-enter the password.");
                Console.Write("Password: ");
                secondPassword = GetHiddenConsoleInput();
                Console.WriteLine();

                if (!password.Equals(secondPassword))
                {
                    BasicOutputMessages.PrintErrorMessage("Given posswords doesn't match! Please try again!");
                    password = null;
                }
            }

            Console.WriteLine();
            BasicOutputMessages.PrintSuccessMessage("Password passed the validation!");

            var hashedPassword = PasswordService.EncodePassword(password);

            return hashedPassword;
        }

        public Account GetAccountByUserName(string nickName)
        {
            return Task.Run(async () => await AccountService.GetAccountByUserNameAsync(nickName)).Result;
        }

        public List<Account> GetAllAccounts()
        {
            return Task.Run(async () => await AccountService.GetAllAccountsAsync()).Result;
        }

        public bool UpdateAccount(Account account)
        {
            return Task.Run(async () => await AccountService.UpdateAccountAsync(account)).Result;
        }

        public string GetHiddenConsoleInput()
        {
            StringBuilder input = new StringBuilder();
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    break;
                }
                if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input.Remove(input.Length - 1, 1);
                }
                else if (key.Key != ConsoleKey.Backspace)
                {
                    input.Append(key.KeyChar);
                }
            }
            return input.ToString();
        }

        public string CreateName(string request = "Insert your full name.")
        {
            string name;

            Console.WriteLine();
            BasicOutputMessages.PrintResponseMessage(request);
            Console.Write("Name: ");
            name = Console.ReadLine();
            Console.WriteLine();

            return name;
        }

        public Sex ChooseGender(string request = "Pick your gender.")
        {
            string input;

            Console.WriteLine();
            BasicOutputMessages.PrintResponseMessage(request);
            Console.WriteLine("Options: ");
            Console.WriteLine("\t1\tMale");
            Console.WriteLine("\t2\tFemale");
            Console.WriteLine("\t3\tOther");
            Console.Write("Option: ");
            input = Console.ReadLine().Trim();
            Console.WriteLine();
            var gender = input switch
            {
                "1" => Sex.Male,
                "2" => Sex.Female,
                _ => Sex.Other,
            };

            return gender;
        }

        public Currency ChooseCurrency(string request = "Pick currency for your account. If none of the following is picked, first listed currency is used.")
        {
            string input;

            Console.WriteLine();
            BasicOutputMessages.PrintResponseMessage(request);
            var currencies = Task.Run(async () => await CurrencyService.GetAllCurrenciesAsync()).Result;
            var position = 0;

            Console.WriteLine("Options: ");

            if (currencies.Count == 0)
            {
                Console.WriteLine("No currency exists!");
                // CREATE CURRENCY
            }

            foreach (var curr in currencies)
            {
                position += 1;
                Console.WriteLine($"\t{position}\t{curr.Name} [{curr.ShortName}]");
            }

            Console.Write("Option: ");
            input = Console.ReadLine().Trim();
            Console.WriteLine();
            Currency currency = null;

            if (!int.TryParse(input, out var parsedInput) || parsedInput <= 0 || parsedInput > currencies.Count)
            {
                currency = currencies[0];
            } else
            {
                currency = currencies[parsedInput - 1];
            }

            BasicOutputMessages.PrintResponseMessage($"Choosen currency: {currency.Name} [{currency.ShortName}]");
            Console.WriteLine();

            return currency;
        }

        private bool ValidateUsername(string username)
        {
            if (username == null)
            {
                return false;
            }

            var result = Task.Run(async () => await AccountService.ValidateUsernameAsync(username)).Result;

            if (!result)
            {
                BasicOutputMessages.PrintErrorMessage("This username is already used. Please choose different name.");
                return false;
            }

            BasicOutputMessages.PrintSuccessMessage("Pass");
            return true;
        }

        public void CoppyAccount(Account source, Account destination)
        {
            destination.Id = source.Id;
            destination.UserName = source.UserName;
            destination.PasswordHash = source.PasswordHash;
            destination.Name = source.Name;
            destination.Gender = source.Gender;
            destination.Ballance = source.Ballance;
            destination.Currency = source.Currency;
            destination.LastTimeLogedIn = source.LastTimeLogedIn.Value;
        }
    }
}
