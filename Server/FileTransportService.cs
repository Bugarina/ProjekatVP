using Common;
using Common.Models;
using DataBase;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class FileTransportService : IFileTransport
    {
        public static DatabaseXML db = new DatabaseXML();
        public delegate List<Load> CalculationDelegateHandler(object sender, List<Load> args);
        Calculation calculation = new Calculation();
           

        public void Calculate()
        {
            List<Load> args = new List<Load>();
            args = db.Read(ConfigurationManager.AppSettings["DBLoads"]);
            args = calculation.InvokeEvent(args);
            db.WriteCalculation(args, ConfigurationManager.AppSettings["DBLoads"]);
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public bool ParseFile(FileManipulationOptions options,bool isForecast, out List<Audit> errors)
        {
            errors = new List<Audit>();
            List<Load> values = new List<Load>();
            int line = 1;

            using (StreamReader stream = new StreamReader(options.MS))
            {
                string data = stream.ReadToEnd();
                string[] csv_rows = data.Split('\n');
                string[] rows = csv_rows.Take(csv_rows.Length - 1).ToArray();

                foreach (var row in rows)
                {
                    char[] delimiters = { ',' };
                    string[] rowSplit = row.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);


                    if (csv_rows.Length > 26 || csv_rows.Length < 23)
                    {
                        errors.Add(
                               new Audit(0, DateTime.Now, MessageType.Error, "Invalid number of rows in CSV file " + DateTime.Now.ToString("yyyy-MM-dd HH-mm"))
                           );
                    }
                    else if (rowSplit.Length != 2)
                    {
                        errors.Add(
                                new Audit(0, DateTime.Now, MessageType.Error, "Invalid data format in CSV file " + DateTime.Now.ToString("yyyy-MM-dd HH-mm"))
                            );
                    }
                    else
                    {
                        if (!DateTime.TryParse(rowSplit[0], out DateTime vreme))
                        {
                            errors.Add(
                                new Audit(0, DateTime.Now, MessageType.Error, "Invalid Timestamp for date " + DateTime.Now.ToString("yyyy-MM-dd HH-mm"))
                            );
                        }
                        else
                        {
                            if (!double.TryParse(rowSplit[1].Replace(',', '.'), out double vrednost))
                            {
                                errors.Add(
                                new Audit(0, DateTime.Now, MessageType.Error, "Invalid Measured Value for date " + vreme.ToString("yyyy-MM-dd HH-mm"))
                            );
                            }
                            else
                            {
                                if (vrednost < 0.0)
                                {
                                    errors.Add(
                                        new Audit(0, DateTime.Now, MessageType.Error, "Invalid Measured Value for date " + vreme.ToString("yyyy-MM-dd"))
                                    );

                                }

                                else
                                {
                                    if (!isForecast)
                                    {

                                        values.Add(
                                            new Load(1, vreme, vrednost, -1)
                                        );
                                    }
                                    else
                                    {
                                    values.Add(
                                       new Load(1, vreme, -1, vrednost) 
                                    );
                                    }
                                }
                            }
                        }
                    }
                    line++;
                }
                stream.Dispose();
            }
            if (errors.Count == line - 1)
            {
                errors.Clear();
                errors.Add(
                        new Audit(0, DateTime.Now, MessageType.Error, "Invalid datastructure in CSV file " + DateTime.Now.ToString("yyyy-MM-dd"))
                );

                return false;
            }

            db.Write(values, errors, ConfigurationManager.AppSettings["DBLoads"], ConfigurationManager.AppSettings["DBAudits"]);
            Calculate();

            if (errors.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


    }




}
