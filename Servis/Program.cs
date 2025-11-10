using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Servis
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost svc = new ServiceHost(typeof(Library));
            svc.Open();

            Console.WriteLine("The service started successfully. Press [Enter] to stop this service.");

            Console.ReadLine();            svc.Close();
        }
    }
}
