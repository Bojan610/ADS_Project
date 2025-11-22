using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Servis
{
    public class Library : ILibrary, ISecurityMechanisms
    {
        static readonly UserRepository userRepository = new UserRepository();

        public string Authentification(string username, string password)
        {
            return userRepository.AuthentificateUser(username, password);
        }

        public bool GetMember(string token, long jmbg, out Member member)
        {
            userRepository.UserAuthentificated(token);
            userRepository.UserAuthorized(token, EAccessMode.Read);
            
            if (Database.members.ContainsKey(jmbg))
            {
                member = Database.members[jmbg];
                return true;
            }
            else
            {
                LibraryException iz = new LibraryException();
                iz.Reason = "\nFailed to get member. Member does not exist.";
                throw new FaultException<LibraryException>(iz);
            }
        }

        public void AddMember(string token, Member member)
        {
            userRepository.UserAuthentificated(token);
            userRepository.UserAuthorized(token, EAccessMode.Write);

            if (!Database.members.ContainsKey(member.Jmbg))
                Database.members.Add(member.Jmbg, member);
            else
            {
                LibraryException iz = new LibraryException();
                iz.Reason = "\nFailed to add member. Member already exists.";
                throw new FaultException<LibraryException>(iz);
            }
        }

        public bool AddBookToMember(string token, long jmbg, params string[] books)
        {
            userRepository.UserAuthentificated(token);
            userRepository.UserAuthorized(token, EAccessMode.Write);

            if (Database.members.ContainsKey(jmbg))
            {
                foreach (string item in books)
                {
                    Database.members[jmbg].Books.Add(item);
                }
                return true;
            }
            else
            {
                LibraryException iz = new LibraryException();
                iz.Reason = "\nFailed to add books to the member. Member does not exist.";
                throw new FaultException<LibraryException>(iz);
            }
        }

        public void RemoveMember(string token, long jmbg)
        {
            userRepository.UserAuthentificated(token);
            userRepository.UserAuthorized(token, EAccessMode.Write);

            if (Database.members.ContainsKey(jmbg))
                Database.members.Remove(jmbg);
            else
            {
                LibraryException iz = new LibraryException();
                iz.Reason = "\nFailed to remove member. Member does not exist.";
                throw new FaultException<LibraryException>(iz);
            }
        }

        public void ModifyMember(string token, Member member)
        {
            userRepository.UserAuthentificated(token);
            userRepository.UserAuthorized(token, EAccessMode.Write);

            if (Database.members.ContainsKey(member.Jmbg))
            {
                Database.members[member.Jmbg].Firstname = member.Firstname;
                Database.members[member.Jmbg].Lastname = member.Lastname;
            }    
            else
            {
                LibraryException iz = new LibraryException();
                iz.Reason = "\nFailed to modify member. Member does not exist.";
                throw new FaultException<LibraryException>(iz);
            }
        }

        public bool RemoveBookFromMember(string token, long jmbg, int bookNum)
        {
            userRepository.UserAuthentificated(token);
            userRepository.UserAuthorized(token, EAccessMode.Write);

            if (Database.members.ContainsKey(jmbg))
            {       
                Database.members[jmbg].Books.RemoveAt(bookNum - 1);              
                return true;
            }
            else
            {
                LibraryException iz = new LibraryException();
                iz.Reason = "\nFailed to remove books from the member. Member does not exist.";
                throw new FaultException<LibraryException>(iz);
            }
        }

        public List<Member> GetAllMembers(string token)
        {
            userRepository.UserAuthentificated(token);
            userRepository.UserAuthorized(token, EAccessMode.Read);

            List<Member> list = new List<Member>();
            foreach (Member item in Database.members.Values)
            {
                list.Add(item);
            }

            return list;
        }

        public void WriteDatabase(string token, Dictionary<long, Member> members)
        {
            userRepository.UserAuthentificated(token);
            userRepository.UserAuthorized(token, EAccessMode.Sync);

            Console.WriteLine(DateTime.Now.ToString() + " - Database entry initialized.");
            Console.WriteLine($"Replicated {members.Count} members.");

            Database.members = members;
        }

        public Dictionary<long, Member> ReadDatabase(string token, DateTime vreme)
        {
            userRepository.UserAuthentificated(token);
            userRepository.UserAuthorized(token, EAccessMode.Sync);

            Console.WriteLine(DateTime.Now.ToString() + " - Database download initialized.");
            Dictionary<long, Member> retDic = new Dictionary<long, Member>();
            foreach (Member item in Database.members.Values)
            {
                if (item.ModifyTime >= vreme)
                    retDic.Add(item.Jmbg, item);
            }

            return retDic;
        }

    }
}
