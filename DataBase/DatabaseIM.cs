using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public class DatabaseIM
    {

        public Dictionary<int,Load> WriteLoad(Dictionary<int,Load> retLoads, List<Load> loads)
        {
            bool update = false;
            int maxId = retLoads.Count;
            foreach(Load l1 in loads)
            {
                foreach(Load l in retLoads.Values)
                {
                    if(l1.Timestamp == l.Timestamp)
                    {
                        if(l1.ForecastValue == -1)
                        {
                            l.MeasuredValue = l1.MeasuredValue;
                        }
                        else
                        {
                            l.ForecastValue = l1.ForecastValue;
                        }
                        update = true;
                    }
                }
                if (!update)
                {
                    retLoads.Add(++maxId, l1);
                    update = false;
                }
            }


            return retLoads;
        }


          public Dictionary<int,Audit> WriteAudit(Dictionary<int, Audit> retAudits, List<Audit> audits, string fileName)
          {
            int maxID = retAudits.Count;
            if(audits.Count == 0)
            {
                Audit a = new Audit(++maxID,DateTime.Now,MessageType.Info,"Datoteka "+fileName+" je uspesno procitana");
                retAudits.Add(++maxID,a);
            }

                foreach(Audit audit in audits)
                {

                    retAudits.Add(++maxID, audit);
                    
                }

            return retAudits;
          }


          public Dictionary<int,ImportedFile> WriteImportedFile(Dictionary<int, ImportedFile> retFiles, List<ImportedFile> files)
        {
            int maxID = retFiles.Count;
            foreach(ImportedFile file in files)
            {
                retFiles.Add(++maxID,file);
            }

            return retFiles;
        }

          public Dictionary<int,Load> WriteCalculation(Dictionary<int,Load> retLoads, List<Load> loads)
        {
            foreach (Load l1 in loads)
            {
                foreach (Load l in retLoads.Values)
                {
                    if (l1.Timestamp == l.Timestamp)
                    {
                        l.AbsolutePercentageDeviation = l1.AbsolutePercentageDeviation;
                        l.SquaredDeviation = l1.SquaredDeviation;
                    }
                }

            }
            return retLoads;
        }


    }
}
