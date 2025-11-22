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
            string token = logIn();
            showMenu(token);    
        }

        static string logIn()
        {
            ChannelFactory<ISecurityMechanisms> channelSecurity = new ChannelFactory<ISecurityMechanisms>("Security");
            ISecurityMechanisms proxySecurity = channelSecurity.CreateChannel();
            string token = "";

            //login to the system
            Console.Title = "Login Portal";

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================");
            Console.WriteLine("          LOGIN PORTAL");
            Console.WriteLine("==================================\n");
            Console.ResetColor();
            string username = "";
            string password = "";

            while (true)
            {
                try
                {
                    Console.WriteLine("Username:");
                    username = Console.ReadLine();
                    Console.WriteLine("Password:");
                    password = Console.ReadLine();

                    Console.WriteLine("\nValidating credentials...");
                    token = proxySecurity.Authentification(username, password);
                    Thread.Sleep(2000);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Login successful! Welcome, " + username + ".\n");
                    Thread.Sleep(1000);
                    Console.ResetColor();
                    return token;
                }
                catch (FaultException<SecurityException> iz)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(iz.Detail.Reason);
                    Console.ResetColor();
                }
            }
        }

        static void showMenu(string token)
        {
            ChannelFactory<ILibrary> channelLibrary = new ChannelFactory<ILibrary>("Library");
            ILibrary proxyLibrary = channelLibrary.CreateChannel();
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("==================================");
                Console.WriteLine("            MAIN MENU");
                Console.WriteLine("==================================");
                Console.ResetColor();

                Console.WriteLine("1. View all members");
                Console.WriteLine("2. View member");
                Console.WriteLine("3. Add member");
                Console.WriteLine("4. Remove member");
                Console.WriteLine("5. Modify member");
                Console.WriteLine("6. Add books to the member");
                Console.WriteLine("7. Remove books from the member");
                Console.WriteLine("8. Log Out");
                Console.WriteLine("----------------------------------");
                Console.Write("Select an option (1-8): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllMembers(proxyLibrary, token);
                        break;
                    case "2":
                        ShowMember(proxyLibrary, token);
                        break;
                    case "3":
                        AddMember(proxyLibrary, token);
                        break;
                    case "4":
                        RemoveMember(proxyLibrary, token);
                        break;
                    case "5":
                        ModifyMember(proxyLibrary, token);
                        break;
                    case "6":
                        AddBookToMember(proxyLibrary, token);
                        break;
                    case "7":
                        RemoveBookFromMember(proxyLibrary, token);
                        break;
                    case "8":
                        Console.WriteLine("\nLogging out...");
                        exit = true;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nInvalid choice. Please try again.");
                        Console.ResetColor();
                        break;
                }
            }
        }
        static void ShowAllMembers(ILibrary proxyLibrary, string token)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================");
            Console.WriteLine("            ALL MEMBERS");
            Console.WriteLine("==================================");
            Console.ResetColor();

            try
            {
                List<Member> members = proxyLibrary.GetAllMembers(token);
                if (members.Any())
                {
                    Console.WriteLine("\nMembers:\n");
                    foreach (Member member in proxyLibrary.GetAllMembers(token))
                    {
                        Console.WriteLine(member.ToString());
                    }
                } 
                else
                {
                    Console.WriteLine("\nThere is no any member in the system.");
                }      
            }
            catch (FaultException<SecurityException> biz)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(biz.Detail.Reason);
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        static void AddMember(ILibrary proxyLibrary, string token)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================");
            Console.WriteLine("            ADDING NEW MEMBER");
            Console.WriteLine("==================================");
            Console.ResetColor();

            Console.WriteLine("First name:");
            string firstName = Console.ReadLine();
            Console.WriteLine("Last name:");
            string lastName = Console.ReadLine();

            Console.WriteLine("JMBG:");
            string input = Console.ReadLine();
            long jmbg;

            while (!long.TryParse(input, out jmbg) || input.Length != 13)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("JMBG must be a 13-digit number. Try again:");
                Console.ResetColor();

                input = Console.ReadLine();
            }

            try
            {
                Member member = new Member(firstName, lastName, jmbg);
                proxyLibrary.AddMember(token, member);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nMember " + member.Firstname + " successfully added.");
                Console.ResetColor();
            }
            catch (FaultException<LibraryException> iz)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(iz.Detail.Reason);
                Console.ResetColor();
            }
            catch (FaultException<SecurityException> biz)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(biz.Detail.Reason);
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        static void ShowMember(ILibrary proxyLibrary, string token)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================");
            Console.WriteLine("            MEMBER");
            Console.WriteLine("==================================");
            Console.ResetColor();

            Console.WriteLine("JMBG:");
            string input = Console.ReadLine();
            long jmbg;

            while (!long.TryParse(input, out jmbg) || input.Length != 13)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("JMBG must be a 13-digit number. Try again:");
                Console.ResetColor();

                input = Console.ReadLine();
            }

            try
            {
                proxyLibrary.GetMember(token, jmbg, out Member member);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nThe member successfuly retrieved:");
                Console.ResetColor();
                Console.WriteLine(member.ToString());
            }
            catch (FaultException<LibraryException> iz)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(iz.Detail.Reason);
                Console.ResetColor();
            }
            catch (FaultException<SecurityException> biz)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(biz.Detail.Reason);
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        static void RemoveMember(ILibrary proxyLibrary, string token)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================");
            Console.WriteLine("            REMOVE MEMBER");
            Console.WriteLine("==================================");
            Console.ResetColor();

            Console.WriteLine("JMBG:");
            string input = Console.ReadLine();
            long jmbg;

            while (!long.TryParse(input, out jmbg) || input.Length != 13)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("JMBG must be a 13-digit number. Try again:");
                Console.ResetColor();

                input = Console.ReadLine();
            }

            try
            {
                proxyLibrary.RemoveMember(token, jmbg);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nMember successfully removed.");
                Console.ResetColor();
            }
            catch (FaultException<LibraryException> iz)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(iz.Detail.Reason);
                Console.ResetColor();
            }
            catch (FaultException<SecurityException> biz)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(biz.Detail.Reason);
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        static void ModifyMember(ILibrary proxyLibrary, string token)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================");
            Console.WriteLine("            MODIFY MEMBER");
            Console.WriteLine("==================================");
            Console.ResetColor();

            Console.WriteLine("JMBG:");
            string input = Console.ReadLine();
            long jmbg;

            while (!long.TryParse(input, out jmbg) || input.Length != 13)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("JMBG must be a 13-digit number. Try again:");
                Console.ResetColor();

                input = Console.ReadLine();
            }

            Console.WriteLine("New first name:");
            string firstName = Console.ReadLine();
            Console.WriteLine("New last name:");
            string lastName = Console.ReadLine();

            try
            {
                Member member = new Member(firstName, lastName, jmbg);
                proxyLibrary.ModifyMember(token, member);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nThe member successfuly modified:");
                Console.ResetColor();
            }
            catch (FaultException<LibraryException> iz)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(iz.Detail.Reason);
                Console.ResetColor();
            }
            catch (FaultException<SecurityException> biz)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(biz.Detail.Reason);
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        public static void AddBookToMember(ILibrary proxyLibrary, string token)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================");
            Console.WriteLine("            ADD BOOKS TO THE MEMBER");
            Console.WriteLine("==================================");
            Console.ResetColor();

            Console.WriteLine("JMBG:");
            string input = Console.ReadLine();
            long jmbg;

            while (!long.TryParse(input, out jmbg) || input.Length != 13)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("JMBG must be a 13-digit number. Try again:");
                Console.ResetColor();

                input = Console.ReadLine();
            }

            List<string> books = new List<string>();
            Console.WriteLine("\nEnter 'finish' to finish book adding");

            while (true)
            {
                Console.WriteLine("Book name: ");
                input = Console.ReadLine();

                if (input.ToLower() == "finish")              
                    break;
                
                books.Add(input);
            }

            try
            {
                proxyLibrary.AddBookToMember(token, jmbg, books.ToArray());
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nBooks successfully added to the member.");
                Console.ResetColor();
            }
            catch (FaultException<LibraryException> iz)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(iz.Detail.Reason);
                Console.ResetColor();
            }
            catch (FaultException<SecurityException> biz)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(biz.Detail.Reason);
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        public static void RemoveBookFromMember(ILibrary proxyLibrary, string token)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================");
            Console.WriteLine("            REMOVE BOOKS FROM THE MEMBER");
            Console.WriteLine("==================================");
            Console.ResetColor();

            Console.WriteLine("JMBG:");
            string input = Console.ReadLine();
            long jmbg;

            while (!long.TryParse(input, out jmbg) || input.Length != 13)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("JMBG must be a 13-digit number. Try again:");
                Console.ResetColor();

                input = Console.ReadLine();
            }       

            try
            {
                Member member;
                proxyLibrary.GetMember(token, jmbg, out member);

                if (member.Books.Count > 0)
                {
                    int bookNum = 0;
                    Console.WriteLine("\nEnter 'finish' to finish removing books from the member");

                    while (true)
                    {
                        proxyLibrary.GetMember(token, jmbg, out member);
                        Console.WriteLine(member.ToString());
                        if (member.Books.Count == 0) 
                            break;
                                                              
                        Console.WriteLine("Enter the book number to delete:");
                        input = Console.ReadLine();

                        if (input.ToLower() == "finish")
                            break;

                        while (!int.TryParse(input, out bookNum) || bookNum < 1 || bookNum > member.Books.Count)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Invalid book number. Please enter the number between 1 and {member.Books.Count}. Try again:");
                            Console.ResetColor();
                            input = Console.ReadLine();
                        }

                        proxyLibrary.RemoveBookFromMember(token, jmbg, bookNum);                       
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nBook successfully removed from the member.");
                        Console.ResetColor();                 
                    }
                }
                else
                {
                    Console.WriteLine("\n" + member.ToString());
                }
            }
            catch (FaultException<LibraryException> iz)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(iz.Detail.Reason);
                Console.ResetColor();
            }
            catch (FaultException<SecurityException> biz)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(biz.Detail.Reason);
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }
    }
}
