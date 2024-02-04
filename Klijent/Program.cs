using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Klijent
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<IBezbednosniMehanizmi> factory1 = new ChannelFactory<IBezbednosniMehanizmi>("Bezbednost");
            IBezbednosniMehanizmi proxy1 = factory1.CreateChannel();
            string token = "";

            try
            {
                Console.WriteLine("Unesite korisnicko ime:");
                string username = Console.ReadLine();
                Console.WriteLine("Unesite lozinku:");
                string password = Console.ReadLine();
                token = proxy1.Autentifikacija(username, password);
            }
            catch (FaultException<BezbednosniIzuzetak> iz)
            {
                Console.WriteLine(iz.Detail.Razlog);
            }


            ChannelFactory<IBiblioteka> factory = new ChannelFactory<IBiblioteka>("Biblioteka");
            IBiblioteka proxy = factory.CreateChannel();            Clan clan;

            while (true)
            {
                //dodavanje clana
                try
                {
                    clan = new Clan("Bojan", "Brdarevic", 0000);
                    proxy.DodajClana(token, clan);
                    Console.WriteLine("Clan " + clan.Ime + " uspesno dodat.");
                }
                catch (FaultException<BibliotekaIzuzetak> iz)
                {
                    Console.WriteLine(iz.Detail.Razlog);
                }
                catch (FaultException<BezbednosniIzuzetak> biz)
                {
                    Console.WriteLine(biz.Detail.Razlog);
                }

                try
                {
                    clan = new Clan("Bojan", "Brdarevic", 0000);
                    proxy.DodajClana(token, clan);
                    Console.WriteLine("Clan " + clan.Ime + " uspesno dodat.");
                }
                catch (FaultException<BibliotekaIzuzetak> iz)
                {
                    Console.WriteLine(iz.Detail.Razlog);
                }
                catch (FaultException<BezbednosniIzuzetak> biz)
                {
                    Console.WriteLine(biz.Detail.Razlog);
                }

                //dodavanje knjiga clanu
                try
                {
                    proxy.DodajKnjiguClanu(token, 0000, "Na drini cuprija", "Mali princ", "Uspeh, sto da ne");
                    Console.WriteLine("Uspesno dodavanje knjiga clanu.");
                }
                catch (FaultException<BibliotekaIzuzetak> iz)
                {
                    Console.WriteLine(iz.Detail.Razlog);
                }
                catch (FaultException<BezbednosniIzuzetak> biz)
                {
                    Console.WriteLine(biz.Detail.Razlog);
                }

                try
                {
                    proxy.DodajKnjiguClanu(token, 1111, "Na drini cuprija", "Mali princ", "Uspeh, sto da ne");
                    Console.WriteLine("Uspesno dodavanje knjiga clanu.");
                }
                catch (FaultException<BibliotekaIzuzetak> iz)
                {
                    Console.WriteLine(iz.Detail.Razlog);
                }
                catch (FaultException<BezbednosniIzuzetak> biz)
                {
                    Console.WriteLine(biz.Detail.Razlog);
                }

                //dobavi clana
                try
                {
                    proxy.DobaviClana(token, 0000, out clan);
                    Console.WriteLine("Clan uspesno dobavljen:\n" + clan.ToString());
                }
                catch (FaultException<BibliotekaIzuzetak> iz)
                {
                    Console.WriteLine(iz.Detail.Razlog);
                }
                catch (FaultException<BezbednosniIzuzetak> biz)
                {
                    Console.WriteLine(biz.Detail.Razlog);
                }

                try
                {
                    proxy.DobaviClana(token, 1111, out clan);
                    Console.WriteLine("Clan uspesno dobavljen:\n" + clan.ToString());
                }
                catch (FaultException<BibliotekaIzuzetak> iz)
                {
                    Console.WriteLine(iz.Detail.Razlog);
                }
                catch (FaultException<BezbednosniIzuzetak> biz)
                {
                    Console.WriteLine(biz.Detail.Razlog);
                }

                //obrisi knjigu clanu
                try
                {
                    proxy.ObrisiKnjiguClanu(token, 0000, "Na drini cuprija", "Uspeh, sto da ne");
                    Console.WriteLine("Uspeno brisanje knjiga clanu.");
                }
                catch (FaultException<BibliotekaIzuzetak> iz)
                {
                    Console.WriteLine(iz.Detail.Razlog);
                }
                catch (FaultException<BezbednosniIzuzetak> biz)
                {
                    Console.WriteLine(biz.Detail.Razlog);
                }

                //svi clanovi
                try
                {
                    Console.WriteLine("Spisak svih clanova:\n");
                    foreach (Clan item in proxy.SviClanovi(token))
                    {
                        Console.WriteLine(item.ToString());
                    }
                }
                catch (FaultException<BezbednosniIzuzetak> biz)
                {
                    Console.WriteLine(biz.Detail.Razlog);
                }

                Thread.Sleep(3000);
            }
          
        }
    }
}
