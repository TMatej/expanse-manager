using ExpanseManager.Controller;
using System;
using System.Threading.Tasks;

namespace ExpanseManager
{
    class Program
    {
        static Task Main(string[] args)
        {
            ApplicationManager manager = new ApplicationManager();
            return manager.StartAsync();
        }
    }
}
