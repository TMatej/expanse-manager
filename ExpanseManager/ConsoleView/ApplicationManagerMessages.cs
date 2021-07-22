
namespace ExpanseManager.ConsoleView
{
    public class ApplicationManagerMessages
    {
        private const string RootNotSetUpMessage = "It looks like your root account is not set up! Let's start with that first.";
        private const string WelcomeMessage = "Welcome to the Expanse Manager, place where all your transaction history awaits. Due to underpayment of employees is current project little bit behind the schedule :/.";
        private const string ApplicationCommands = "Available commands:\n" +
            " login\tLog in as user.\n" +
            " new\tCreate new account.\n" +
            " quit\tEnd the program.\n" +
            "\nPrintout commands:\n\n" +
            " root\tShow root account information in JSON.\n" +
            " users\tPrints accounts info in JSON (just for debug version) .";
        private const string GoodByeMessage = "Thank you for using my application. See ya soon.\nBye!";

        public static void PrintRootNotSetUpMessage()
        {
            BasicOutputMessages.PrintResponseMessage(RootNotSetUpMessage);
            BasicOutputMessages.PrintAcknowledgeMessage();
        }

        public static void PrintAccountAddedMessage()
        {
            BasicOutputMessages.PrintSuccessMessage("Success! Account added to the list!");
        }

        public static void PrintWelcomeMessage()
        {
            BasicOutputMessages.PrintResponseMessage(WelcomeMessage);
        }

        public static void PrintApplicationCommands()
        {
            BasicOutputMessages.PrintResponseMessage(ApplicationCommands);
        }

        public static void PrintGoodByeMessage()
        {
            BasicOutputMessages.PrintResponseMessage(GoodByeMessage);
        }
    }
}
