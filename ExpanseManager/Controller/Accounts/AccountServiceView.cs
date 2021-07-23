using ExpanseManager.ConsoleView;
using ExpanseManager.Controller.Accounts;
using ExpanseManagerDBLibrary.Models;
using ExpanseManagerDBLibrary.Models.Enums;
using ExpanseManagerServiceLibrary.Services.Accounts;
using ExpanseManagerServiceLibrary.Services.Currencies;
using ExpanseManagerServiceLibrary.Services.Security;
using System;
using System.Collections.Generic;
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

        public async Task<AccountModel> CreateNewAccountAsync()
        {
            Console.Clear();
            BasicOutputMessages.PrintResponseMessage("To create new account please fill in the request. All information can be changed after logging into your account.");
            Console.WriteLine();
            string username = await CreateUserNameAsync();
            string password = CreatePassword();
            string name = AccountUtils.CreateName();
            Sex gender = AccountUtils.ChooseGender();
            CurrencyModel currency = await ChooseCurrencyAsync();
            AccountModel account = await CreateAccountAsync(username, password, name, gender, currency);

            return account;
        }

        public async Task<AccountModel> CreateRootAccountAsync()
        {
            Console.Clear();
            BasicOutputMessages.PrintResponseMessage("To create a root account, please follow these few steps.");
            string username = "root";
            string password = CreatePassword();
            string name = "Application root account";
            Sex gender = Sex.Other;
            CurrencyModel currency = await ChooseCurrencyAsync();

            AccountModel account = await CreateAccountAsync(username, password, name, gender, currency);

            return account;
        }

        private async Task<AccountModel> CreateAccountAsync(string username, string password, string name, Sex gender, CurrencyModel currency)
        {
            AccountModel newAccount = new()
            {
                UserName = username,
                PasswordHash = password,
                Name = name,
                Gender = gender,
                Ballance = 0L,
                Currency = currency
            };

            newAccount = await AccountService.CreateAccountAsync(newAccount);
            BasicOutputMessages.PrintSuccessMessage("Success! Account created!");
            Console.WriteLine();
            BasicOutputMessages.PrintResponseMessage("Sum of created account in JSON:");
            Console.WriteLine(newAccount.ToJSON());
            Console.WriteLine();
            return newAccount;
        }

        public async Task<string> CreateUserNameAsync(string request = "Choose a unique username.")
        {
            string username = null;

            while (!await ValidateUsernameAsync(username))
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
                    password = ViewUtils.GetHiddenConsoleInput();
                    Console.WriteLine();
                }

                Console.WriteLine();
                BasicOutputMessages.PrintResponseMessage("Please, re-enter the password.");
                Console.Write("Password: ");
                secondPassword = ViewUtils.GetHiddenConsoleInput();
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
                                
        public async Task<CurrencyModel> ChooseCurrencyAsync(string request = "Pick currency for your account. If none of the following is picked, first listed currency is used.")
        {
            string input;

            Console.WriteLine();
            BasicOutputMessages.PrintResponseMessage(request);
            var currencies = await CurrencyService.GetAllCurrenciesAsync();
            var position = 0;

            Console.WriteLine("Options: ");

            if (currencies.Count == 0)
            {
                Console.WriteLine("No currency exists!");
            }

            foreach (var curr in currencies)
            {
                position += 1;
                Console.WriteLine($"\t{position}\t{curr.Name} [{curr.ShortName}]");
            }

            Console.Write("Option: ");
            input = Console.ReadLine().Trim();
            Console.WriteLine();
            CurrencyModel currency = null;

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

        private async Task<bool> ValidateUsernameAsync(string username)
        {
            if (username == null)
            {
                return false;
            }

            var result = await AccountService.ValidateUsernameAsync(username);

            if (!result)
            {
                BasicOutputMessages.PrintErrorMessage("This username is already used. Please choose different name.");
                return false;
            }

            BasicOutputMessages.PrintSuccessMessage("Pass");
            return true;
        }
    }
}
