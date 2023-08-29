using Common;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ProjVP2
{
    public class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {

            Console.WriteLine("=============================================================");
            Console.WriteLine("Please enter the path to the files you want to read");
            Console.WriteLine("To exit the app enter exit");

                string folderPath = Console.ReadLine();

                if (folderPath.ToLower() == "exit")
                {
                    break;
                }

                if (Directory.Exists(folderPath))
                {
                    foreach (string filePath in Directory.GetFiles(folderPath, "*.csv"))
                    {
                        string fileName = Path.GetFileName(filePath);


                        if (fileName.StartsWith("forecast"))
                        {
                            List<Audit> errors = new List<Audit>();

                            errors = SendCSV(filePath,true);
                            Console.WriteLine("Data successfully entered into database!");
                            foreach (Audit audit in errors)
                            {
                                Console.WriteLine(audit.Message);
                            }
                        }
                        else if (fileName.StartsWith("measured"))
                        {
                            List<Audit> errors = new List<Audit>();

                            errors = SendCSV(filePath,false);
                            Console.WriteLine("Data successfully entered into database!");
                            foreach (Audit audit in errors)
                            {
                                Console.WriteLine(audit.Message);
                            }
                        }
                       
                    }

                    Console.WriteLine("Import completed successfully.");
                }
                else
                {
                    Console.WriteLine("Folder does not exist.");
                }
            }

        }

        private static List<Audit> SendCSV(string filePath, bool isForecast)
        {
            bool success = false;

            MemoryStream memoryStream = new MemoryStream();
            List<Audit> errors = new List<Audit>();

            ChannelFactory<IFileTransport> xmlCsvChannelFactory = new ChannelFactory<IFileTransport>("XmlCsvParser");
            IFileTransport csvProxy = xmlCsvChannelFactory.CreateChannel();


            using (FileStream csvFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                csvFileStream.CopyTo(memoryStream);
                csvFileStream.Dispose();
            }

            memoryStream.Position = 0;

            using (FileManipulationOptions options = new FileManipulationOptions(memoryStream, Path.GetFileName(filePath)))
            {
                success = csvProxy.ParseFile(options,isForecast, out errors);
                options.Dispose();
            }

            return errors;
        }


    }
}
