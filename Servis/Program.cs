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
            ServiceHost svc = new ServiceHost(typeof(Biblioteka));
            svc.Open();

            Console.WriteLine("Server supesno pokrenut. Pritisnite [Enter] za zaustavljanje servisa.");

            Console.ReadLine();            svc.Close();
        }
    }
}
