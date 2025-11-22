using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Replikator
{
    class Program
    {
        static void Main(string[] args)
        {
            var tokens = logIn();
            string tokenSource = tokens.Item1;
            string tokenDestination = tokens.Item2;
            startReplicator(tokenSource, tokenDestination);
        }

        static Tuple<string, string> logIn()
        {
            ChannelFactory<ISecurityMechanisms> channelSecuritySource = new ChannelFactory<ISecurityMechanisms>("SecuritySource");
            ISecurityMechanisms proxySecuritySource = channelSecuritySource.CreateChannel();

            ChannelFactory<ISecurityMechanisms> channelSecurityDest = new ChannelFactory<ISecurityMechanisms>("SecurityDestination");
            ISecurityMechanisms proxySecurityDest = channelSecurityDest.CreateChannel();

            string tokenSource = "";
            string tokenDestination = "";

            //login to the replicator system
            Console.Title = "Replicator Login Portal";

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================");
            Console.WriteLine("          REPLICATOR LOGIN PORTAL");
            Console.WriteLine("==================================\n");
            Console.ResetColor();
            string username = "";
            string password = "";

            while (true)
            {
                try
                {
                    Console.WriteLine("Username:");
                    username = Console.ReadLine();
                    Console.WriteLine("Password:");
                    password = Console.ReadLine();

                    Console.WriteLine("\nValidating credentials...");
                    tokenSource = proxySecuritySource.Authentification(username, password);
                    tokenDestination = proxySecurityDest.Authentification(username, password);
                    Thread.Sleep(2000);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Replicator system started successfully!");
                    Thread.Sleep(1000);
                    Console.ResetColor();
                    return new Tuple<string, string>(tokenSource, tokenDestination);
                }
                catch (FaultException<SecurityException> iz)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(iz.Detail.Reason);
                    Console.ResetColor();
                }
            }
        }

        public static void startReplicator(string tokenSource, string tokenDestination)
        {
            DateTime time = DateTime.Now;   

            while (true)
            {
                try
                {
                    ChannelFactory<ILibrary> channelLibrarySource = new ChannelFactory<ILibrary>("Source");
                    ChannelFactory<ILibrary> channelLibraryDest = new ChannelFactory<ILibrary>("Destination");
                    ILibrary proxyLibrarySource = channelLibrarySource.CreateChannel();
                    ILibrary proxyLibraryDest = channelLibraryDest.CreateChannel();

                    proxyLibraryDest.WriteDatabase(tokenDestination, proxyLibrarySource.ReadDatabase(tokenSource, time));
                    time = DateTime.Now;
                }
                catch (FaultException<SecurityException> ex)
                {
                    Console.WriteLine(ex.Detail.Reason);
                }

                Thread.Sleep(5000);
            }
        }
    }
}
