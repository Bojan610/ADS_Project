using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis
{
    class Korisnik
    {
        string username;
        string password;
        bool autentifikovan = false;
        string token;

        public Korisnik(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public bool Autentifikovan
        {
            get => autentifikovan;
            set => autentifikovan = value;
        }
        public string Token { get => token; set => token = value; }

    }
}
