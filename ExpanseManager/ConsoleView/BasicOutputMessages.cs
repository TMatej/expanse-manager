using System;

namespace ExpanseManager.ConsoleView
{
    public static class BasicOutputMessages
    {  
        private const string AcknowledgeMessage = "To continue press any button...";

        private const string InvalidInputCommandErrorMessage = "'{0}' is invalid input command!\nPlease, pick a command from list of commands!";

        private const string InvalidInputErrorMessage = "'{0}' is invalid input!";

        public static void PrintAcknowledgeMessage()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine();
            Console.WriteLine(AcknowledgeMessage);
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();
        }

        public static void PrintInvalidInputCommandErorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(InvalidInputCommandErrorMessage, message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void PrintInvalidInputErorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(InvalidInputErrorMessage, message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void PrintResponseMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void PrintSuccessMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void PrintErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
