using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis
{
    class User
    {
        string username;
        string password;
        bool authentificated = false;
        string token;

        public User(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public bool Authentificated
        {
            get => authentificated;
            set => authentificated = value;
        }
        public string Token { get => token; set => token = value; }

    }
}
