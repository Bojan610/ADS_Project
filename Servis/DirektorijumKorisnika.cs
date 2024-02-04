using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Servis
{
    public enum EPravaPristupa { Citanje, Azuriranje, Replikacija };

    class DirektorijumKorisnika
    {
        private Dictionary<string, Korisnik> korisnici = new Dictionary<string, Korisnik>();
        private Dictionary<string, string> autentifikovani = new Dictionary<string, string>();
        private Dictionary<string, SortedSet<EPravaPristupa>> prava = new Dictionary<string, SortedSet<EPravaPristupa>>();
        private const string _pepper = "P&0myWHq";

        public DirektorijumKorisnika()
        {
            DodajKorisnika("pera", "pera");
            DodajKorisnika("admin", "admin");
            DodajKorisnika("repl", "repl");

            
            SortedSet<EPravaPristupa> peraPrava = new SortedSet<EPravaPristupa>();
            peraPrava.Add(EPravaPristupa.Citanje);
            prava.Add("pera", peraPrava);

            SortedSet<EPravaPristupa> adminPrava = new SortedSet<EPravaPristupa>();
            adminPrava.Add(EPravaPristupa.Citanje);
            adminPrava.Add(EPravaPristupa.Azuriranje);
            prava.Add("admin", adminPrava);

            SortedSet<EPravaPristupa> replPrava = new SortedSet<EPravaPristupa>();
            replPrava.Add(EPravaPristupa.Replikacija);
            prava.Add("repl", replPrava);

        }

        private string KodiranTekst(string lozinka)
        {
            using (var sha = SHA256.Create())
            {
                var computedHash = sha.ComputeHash(
                Encoding.Unicode.GetBytes(lozinka + _pepper));
                return Convert.ToBase64String(computedHash);
            }
        }

        public void DodajKorisnika(string ime, string lozinka)
        {
            korisnici.Add(ime, new Korisnik(ime, KodiranTekst(lozinka)));
        }

        public string AutentifikacijaKorisnika(string ime, string lozinka)
        {
            if (korisnici.ContainsKey(ime) && KodiranTekst(lozinka) == korisnici[ime].Password)
            {
                korisnici[ime].Autentifikovan = true;
                string token = KodiranTekst(ime + _pepper);
                this.autentifikovani.Add(token, ime);
                return token;
            }
            else
            {
                BezbednosniIzuzetak iz = new BezbednosniIzuzetak();
                iz.Razlog = "Neispravno korisnicko ime i/ili lozinka.";
                throw new FaultException<BezbednosniIzuzetak>(iz);
            }
        }

        public bool KorisnikAutentifikovan(string token)
        {
            if (autentifikovani.ContainsKey(token))
                return true;
            else
            {
                BezbednosniIzuzetak iz = new BezbednosniIzuzetak();
                iz.Razlog = "Korisnik nije autentifikovan.";
                throw new FaultException<BezbednosniIzuzetak>(iz);
            }
        }
        public bool KorisnikAutorizovan(string token, EPravaPristupa pravo)
        {
            if (autentifikovani.ContainsKey(token) && prava.ContainsKey(autentifikovani[token]) && prava[autentifikovani[token]].Contains(pravo))
                return true;
            else
            {
                BezbednosniIzuzetak iz = new BezbednosniIzuzetak();
                iz.Razlog = "Korisnik nije autorizovan.";
                throw new FaultException<BezbednosniIzuzetak>(iz);
            }
        }
    }
}
