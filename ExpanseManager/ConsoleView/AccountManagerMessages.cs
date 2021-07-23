
namespace ExpanseManager.ConsoleView
{
    public class AccountManagerMessages
    {
        private const string AccountCommands = "Available commands:\n" +
           "logout\t\tI mean, this is pretty obvious.\n" +
            "add\t\tIncrease your debet.\n" +
            "pay\t\tTransfer money to your comrade.\n" +
            "change\t\tCustomize your account\n" +
            "export\t\tAllows user to export data to specific file in JSON. *May be included in a future release*\n" +
            "history\t\tShows payment history. *May be in a future release*\n" +
            "json\t\tShow account representation in JSON.";

        private const string RootCommands = AccountCommands +
            "\n\nRoot specific comands:\n" +
            "all cur\t\tShow all available currencies.\n" +
            "add cur\t\tAllows root to insert new currencies.\n" +
            "remove cur\t\tAllows root to remove specific currency. *May be included in a future release*\n" +
            "remove acc\t\tAllows root to remove specific account. *May be included in a future release*\n" +
            "revert payment\t\tAllows root to revert specific payment. *May be included in a future release*\n" +
            "+ many others...";

        public static void PrintAccountCommands()
        {
            BasicOutputMessages.PrintResponseMessage(AccountCommands);
        }

        public static void PrintRootCommands()
        {
            BasicOutputMessages.PrintResponseMessage(RootCommands);
        }
    }
}
