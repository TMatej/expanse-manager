using System;

namespace ExpanseManager.ConsoleView
{
    public class AccountEditorMessages
    {
        private const string EditingAbortedMessage = "Edditing aborted!\nNo information changed.";
        private const string EditingDoneMessage = "Edditing successful.";
        private const string EditingCommands = "Available commands:\n" +
            " name\t\tINFO\n" +
            " username\t\tINFO\n" +
            " password\t\tINFO\n" +
            " sex\t\tINFO\n" +
            " currency\t\tINFO\n\n" +
            " abort\t\tINFO\n" +
            " done\t\tINFO\n";

        public static void PrintEditingAbortedMessage()
        {

            Console.WriteLine();
            BasicOutputMessages.PrintResponseMessage(EditingAbortedMessage);
        }

        public static void PrintEditingDoneMessage()
        {
            Console.WriteLine();
            BasicOutputMessages.PrintResponseMessage(EditingDoneMessage);
        }

        public static void PrintEditingCommands()
        {
            BasicOutputMessages.PrintResponseMessage(EditingCommands);
        }
    }
}
