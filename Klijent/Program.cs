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
            ChannelFactory<ISecurityMechanisms> factory1 = new ChannelFactory<ISecurityMechanisms>("Security");
            ISecurityMechanisms proxy1 = factory1.CreateChannel();
            string token = "";

            
                try
                {
                    Console.WriteLine("Enter username:");
                    string username = Console.ReadLine();
                    Console.WriteLine("Enter password:");
                    string password = Console.ReadLine();
                    token = proxy1.Authentification(username, password);
                }
                catch (FaultException<SecurityException> iz)
                {
                    Console.WriteLine(iz.Detail.Reason);
                }
            
           
            Console.WriteLine("Succes login!");
            ChannelFactory<ILibrary> factory = new ChannelFactory<ILibrary>("Library");
            ILibrary proxy = factory.CreateChannel();            Member member;

            while (true)
            {
                //adding the member
                try
                {
                    member = new Member("Bojan", "Brdarevic", 0000);
                    proxy.AddMember(token, member);
                    Console.WriteLine("Member " + member.Firstname + " successfully added.");
                }
                catch (FaultException<LibraryException> iz)
                {
                    Console.WriteLine(iz.Detail.Reason);
                }
                catch (FaultException<SecurityException> biz)
                {
                    Console.WriteLine(biz.Detail.Reason);
                }

                try
                {
                    member = new Member("Bojan", "Brdarevic", 0000);
                    proxy.AddMember(token, member);
                    Console.WriteLine("Member " + member.Firstname + " successfully added.");
                }
                catch (FaultException<LibraryException> iz)
                {
                    Console.WriteLine(iz.Detail.Reason);
                }
                catch (FaultException<SecurityException> biz)
                {
                    Console.WriteLine(biz.Detail.Reason);
                }

                //adding books to the member 
                try
                {
                    proxy.AddBookToMember(token, 0000, "Na drini cuprija", "Mali princ", "Uspeh, sto da ne");
                    Console.WriteLine("Books added successfully to the member.");
                }
                catch (FaultException<LibraryException> iz)
                {
                    Console.WriteLine(iz.Detail.Reason);
                }
                catch (FaultException<SecurityException> biz)
                {
                    Console.WriteLine(biz.Detail.Reason);
                }

                try
                {
                    proxy.AddBookToMember(token, 1111, "Na drini cuprija", "Mali princ", "Uspeh, sto da ne");
                    Console.WriteLine("Books added successfully to the member.");
                }
                catch (FaultException<LibraryException> iz)
                {
                    Console.WriteLine(iz.Detail.Reason);
                }
                catch (FaultException<SecurityException> biz)
                {
                    Console.WriteLine(biz.Detail.Reason);
                }

                //dobavi clana
                try
                {
                    proxy.GetMember(token, 0000, out member);
                    Console.WriteLine("The member successfuly retrieved:\n" + member.ToString());
                }
                catch (FaultException<LibraryException> iz)
                {
                    Console.WriteLine(iz.Detail.Reason);
                }
                catch (FaultException<SecurityException> biz)
                {
                    Console.WriteLine(biz.Detail.Reason);
                }

                try
                {
                    proxy.GetMember(token, 1111, out member);
                    Console.WriteLine("The member successfuly retrieved:\n" + member.ToString());
                }
                catch (FaultException<LibraryException> iz)
                {
                    Console.WriteLine(iz.Detail.Reason);
                }
                catch (FaultException<SecurityException> biz)
                {
                    Console.WriteLine(biz.Detail.Reason);
                }

                //obrisi knjigu clanu
                try
                {
                    proxy.RemoveBookFromMember(token, 0000, "Na drini cuprija", "Uspeh, sto da ne");
                    Console.WriteLine("Books successfully removed from the member.");
                }
                catch (FaultException<LibraryException> iz)
                {
                    Console.WriteLine(iz.Detail.Reason);
                }
                catch (FaultException<SecurityException> biz)
                {
                    Console.WriteLine(biz.Detail.Reason);
                }

                //svi clanovi
                try
                {
                    Console.WriteLine("List of all members:\n");
                    foreach (Member item in proxy.GetAllMembers(token))
                    {
                        Console.WriteLine(item.ToString());
                    }
                }
                catch (FaultException<SecurityException> biz)
                {
                    Console.WriteLine(biz.Detail.Reason);
                }

                Thread.Sleep(3000);
            }
          
        }
    }
}
