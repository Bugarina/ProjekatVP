using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {

                ServiceHost host = new ServiceHost(typeof(FileTransportService));
                host.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine(DateTime.Now);
            Console.WriteLine("Servis je uspesno pokrenut!");
            Console.ReadLine();
        }
    }
}
