using System;
using Dash.AppStart;

namespace Dash
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                StartAppHost(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fatal exception: {0}", ex.Message);
            }
        }

        private static void StartAppHost(string[] args)
        {
            var listeningOn = args.Length == 0 ? "http://*:8080/" : args[0];
            var appHost = new AppHost().Init().Start(listeningOn);
            Console.WriteLine("AppHost Created at {0}, listening on {1}", DateTime.Now, listeningOn);
            Console.WriteLine("\n\nPress any key to exit..");
            Console.ReadKey();
        }
    }
}
