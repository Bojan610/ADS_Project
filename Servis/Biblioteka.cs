using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Servis
{
    public class Biblioteka : IBiblioteka, IBezbednosniMehanizmi
    {
        static readonly DirektorijumKorisnika direktorijum = new DirektorijumKorisnika();

        public string Autentifikacija(string korisnik, string lozinka)
        {
            return direktorijum.AutentifikacijaKorisnika(korisnik, lozinka);
        }

        public bool DobaviClana(string token, long jmbg, out Clan clan)
        {
            direktorijum.KorisnikAutentifikovan(token);
            direktorijum.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);
            
            if (BazaPodataka.clanovi.ContainsKey(jmbg))
            {
                clan = BazaPodataka.clanovi[jmbg];
                return true;
            }
            else
            {
                BibliotekaIzuzetak iz = new BibliotekaIzuzetak();
                iz.Razlog = "Neuspesno dobavljanje clana. Clan ne postoji.";
                throw new FaultException<BibliotekaIzuzetak>(iz);
            }
        }

        public void DodajClana(string token, Clan clan)
        {
            direktorijum.KorisnikAutentifikovan(token);
            direktorijum.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);

            if (!BazaPodataka.clanovi.ContainsKey(clan.Jmbg))
                BazaPodataka.clanovi.Add(clan.Jmbg, clan);
            else
            {
                BibliotekaIzuzetak iz = new BibliotekaIzuzetak();
                iz.Razlog = "Neuspesno dodavanje clana. Clan vec postoji.";
                throw new FaultException<BibliotekaIzuzetak>(iz);
            }
        }

        public bool DodajKnjiguClanu(string token, long jmbg, params string[] knjige)
        {
            direktorijum.KorisnikAutentifikovan(token);
            direktorijum.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);

            if (BazaPodataka.clanovi.ContainsKey(jmbg))
            {
                foreach (string item in knjige)
                {
                    BazaPodataka.clanovi[jmbg].Knjige.Add(item);
                }
                return true;
            }
            else
            {
                BibliotekaIzuzetak iz = new BibliotekaIzuzetak();
                iz.Razlog = "Neuspesno dodavanje knjige clanu. Clan ne postoji.";
                throw new FaultException<BibliotekaIzuzetak>(iz);
            }
        }

        public void IzbrisiClana(string token, long jmbg)
        {
            direktorijum.KorisnikAutentifikovan(token);
            direktorijum.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);

            if (BazaPodataka.clanovi.ContainsKey(jmbg))
                BazaPodataka.clanovi.Remove(jmbg);
            else
            {
                BibliotekaIzuzetak iz = new BibliotekaIzuzetak();
                iz.Razlog = "Neuspesno brisanje clana. Clan ne postoji.";
                throw new FaultException<BibliotekaIzuzetak>(iz);
            }
        }

        public void IzmeniClana(string token, Clan clan)
        {
            direktorijum.KorisnikAutentifikovan(token);
            direktorijum.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);

            if (BazaPodataka.clanovi.ContainsKey(clan.Jmbg))
                BazaPodataka.clanovi[clan.Jmbg] = clan;
            else
            {
                BibliotekaIzuzetak iz = new BibliotekaIzuzetak();
                iz.Razlog = "Neuspesna izmena clana. Clan ne postoji.";
                throw new FaultException<BibliotekaIzuzetak>(iz);
            }
        }

        public bool ObrisiKnjiguClanu(string token, long jmbg, params string[] knjige)
        {
            direktorijum.KorisnikAutentifikovan(token);
            direktorijum.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);

            if (BazaPodataka.clanovi.ContainsKey(jmbg))
            {
                foreach (string item in knjige)
                {
                    BazaPodataka.clanovi[jmbg].Knjige.Remove(item);
                }
                return true;
            }
            else
            {
                BibliotekaIzuzetak iz = new BibliotekaIzuzetak();
                iz.Razlog = "Neuspesno brisanje knjiga clanu. Clan ne postoji.";
                throw new FaultException<BibliotekaIzuzetak>(iz);
            }
        }

        public List<Clan> SviClanovi(string token)
        {
            direktorijum.KorisnikAutentifikovan(token);
            direktorijum.KorisnikAutorizovan(token, EPravaPristupa.Citanje);

            List<Clan> list = new List<Clan>();
            foreach (Clan item in BazaPodataka.clanovi.Values)
            {
                list.Add(item);
            }

            return list;
        }

        public void PosaljiBazu(string token, Dictionary<long, Clan> baza)
        {
            direktorijum.KorisnikAutentifikovan(token);
            direktorijum.KorisnikAutorizovan(token, EPravaPristupa.Replikacija);

            Console.WriteLine(DateTime.Now.ToString() + " - Upis u bazu podataka iniciran.");
            Console.WriteLine($"Replicirano {baza.Count} podataka.");

            BazaPodataka.clanovi = baza;
        }

        public Dictionary<long, Clan> PreuzmiBazu(string token, DateTime vreme)
        {
            direktorijum.KorisnikAutentifikovan(token);
            direktorijum.KorisnikAutorizovan(token, EPravaPristupa.Replikacija);

            Console.WriteLine(DateTime.Now.ToString() + " - Preuzimanje baze podataka inicirano.");
            Dictionary<long, Clan> retDic = new Dictionary<long, Clan>();
            foreach (Clan item in BazaPodataka.clanovi.Values)
            {
                if (item.VremeIzmene >= vreme)
                    retDic.Add(item.Jmbg, item);
            }

            return retDic;
        }

    }
}
