using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Replikator
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime time = DateTime.Now;

            ChannelFactory<ISecurityMechanisms> factory = new ChannelFactory<ISecurityMechanisms>("SecuritySource");
            ISecurityMechanisms proxy = factory.CreateChannel();

            ChannelFactory<ISecurityMechanisms> factory1 = new ChannelFactory<ISecurityMechanisms>("SecurityDestination");
            ISecurityMechanisms proxy1 = factory1.CreateChannel();

            string tokenSource = "";
            string tokenDestination = "";

            try
            {
                Console.WriteLine("Enter username:");
                string username = Console.ReadLine();
                Console.WriteLine("Enter password:");
                string password = Console.ReadLine();
                tokenSource = proxy.Authentification(username, password);
                tokenDestination = proxy1.Authentification(username, password);
            }
            catch (FaultException<SecurityException> iz)
            {
                Console.WriteLine(iz.Detail.Reason);
            }

            while (true)
            {
                try
                {
                    ChannelFactory<ILibrary> cfSource = new ChannelFactory<ILibrary>("Source");
                    ChannelFactory<ILibrary> cfDestination = new ChannelFactory<ILibrary>("Destination");
                    ILibrary proxySource = cfSource.CreateChannel();
                    ILibrary proxyDestination = cfDestination.CreateChannel();

                    proxyDestination.WriteDatabase(tokenDestination, proxySource.ReadDatabase(tokenSource, time));
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
