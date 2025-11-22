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
    public enum EAccessMode { Read, Write, Sync };

    class UserRepository
    {
        private Dictionary<string, User> users = new Dictionary<string, User>();
        private Dictionary<string, string> authentificatedUsers = new Dictionary<string, string>();
        private Dictionary<string, SortedSet<EAccessMode>> accessModes = new Dictionary<string, SortedSet<EAccessMode>>();
        private const string _pepper = "P&0myWHq";

        public UserRepository()
        {
            AddUser("pera", "pera");
            AddUser("admin", "admin");
            AddUser("repl", "repl");

            
            SortedSet<EAccessMode> peraModes = new SortedSet<EAccessMode>();
            peraModes.Add(EAccessMode.Read);
            accessModes.Add("pera", peraModes);

            SortedSet<EAccessMode> adminModes = new SortedSet<EAccessMode>();
            adminModes.Add(EAccessMode.Read);
            adminModes.Add(EAccessMode.Write);
            accessModes.Add("admin", adminModes);

            SortedSet<EAccessMode> replModes = new SortedSet<EAccessMode>();
            replModes.Add(EAccessMode.Sync);
            accessModes.Add("repl", replModes);

        }

        private string EncodeData(string password)
        {
            using (var sha = SHA256.Create())
            {
                var computedHash = sha.ComputeHash(Encoding.Unicode.GetBytes(password + _pepper));
                return Convert.ToBase64String(computedHash);
            }
        }

        public void AddUser(string username, string password)
        {
            users.Add(username, new User(username, EncodeData(password)));
        }

        public string AuthentificateUser(string username, string password)
        {
            if (users.ContainsKey(username) && EncodeData(password) == users[username].Password)
            {
                users[username].Authentificated = true;
                string token = EncodeData(username + _pepper);
                this.authentificatedUsers.Add(token, username);
                return token;
            }
            else
            {
                SecurityException iz = new SecurityException();
                iz.Reason = "Invalid username or password.\n";
                throw new FaultException<SecurityException>(iz);
            }
        }

        public bool UserAuthentificated(string token)
        {
            if (authentificatedUsers.ContainsKey(token))
                return true;
            else
            {
                SecurityException iz = new SecurityException();
                iz.Reason = "\nUser is not authentificated.";
                throw new FaultException<SecurityException>(iz);
            }
        }
        public bool UserAuthorized(string token, EAccessMode mode)
        {
            if (authentificatedUsers.ContainsKey(token) && accessModes.ContainsKey(authentificatedUsers[token]) && accessModes[authentificatedUsers[token]].Contains(mode))
                return true;
            else
            {
                SecurityException iz = new SecurityException();
                iz.Reason = "\nUser is not authorized for this action.";
                throw new FaultException<SecurityException>(iz);
            }
        }
    }
}
