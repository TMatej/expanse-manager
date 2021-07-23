using ExpanseManager.ConsoleView;
using ExpanseManagerDBLibrary.Models;
using ExpanseManagerDBLibrary.Models.Enums;
using System;

namespace ExpanseManager.Controller.Accounts
{
    public class AccountUtils
    {
        public static string CreateName(string request = "Insert your full name.")
        {
            string name;

            Console.WriteLine();
            BasicOutputMessages.PrintResponseMessage(request);
            Console.Write("Name: ");
            name = Console.ReadLine();
            Console.WriteLine();

            return name;
        }

        public static Sex ChooseGender(string request = "Pick your gender.")
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
                        
        public static void CoppyAccount(AccountModel source, AccountModel destination)
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
