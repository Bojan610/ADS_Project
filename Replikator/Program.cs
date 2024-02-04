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
            DateTime vreme = DateTime.Now;

            ChannelFactory<IBezbednosniMehanizmi> factory = new ChannelFactory<IBezbednosniMehanizmi>("BezbednostIzvor");
            IBezbednosniMehanizmi proxy = factory.CreateChannel();

            ChannelFactory<IBezbednosniMehanizmi> factory1 = new ChannelFactory<IBezbednosniMehanizmi>("BezbednostOdrediste");
            IBezbednosniMehanizmi proxy1 = factory1.CreateChannel();

            string tokenIzvor = "";
            string tokenOdrediste = "";

            try
            {
                Console.WriteLine("Unesite korisnicko ime:");
                string username = Console.ReadLine();
                Console.WriteLine("Unesite lozinku:");
                string password = Console.ReadLine();
                tokenIzvor = proxy.Autentifikacija(username, password);
                tokenOdrediste = proxy1.Autentifikacija(username, password);
            }
            catch (FaultException<BezbednosniIzuzetak> iz)
            {
                Console.WriteLine(iz.Detail.Razlog);
            }

            while (true)
            {
                try
                {
                    ChannelFactory<IBiblioteka> cfIzvor = new ChannelFactory<IBiblioteka>("Izvor");
                    ChannelFactory<IBiblioteka> cfOdrediste = new ChannelFactory<IBiblioteka>("Odrediste");
                    IBiblioteka proxyIzvor = cfIzvor.CreateChannel();
                    IBiblioteka proxyOdrediste = cfOdrediste.CreateChannel();

                    proxyOdrediste.PosaljiBazu(tokenOdrediste, proxyIzvor.PreuzmiBazu(tokenIzvor, vreme));
                    vreme = DateTime.Now;
                    
                    
                }
                catch (FaultException<BezbednosniIzuzetak> ex)
                {
                    Console.WriteLine(ex.Detail.Razlog);
                }

                Thread.Sleep(5000);
            }
        }
    }
}
