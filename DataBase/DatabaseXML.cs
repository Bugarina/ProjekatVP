using Common;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
namespace DataBase
{
    public class DatabaseXML
    {
        private static int ID = 1;

        public List<Load> Read(string path)
        {
            List<Load> loads = new List<Load>();

            using (FileManipulationOptions options = OpenFile(path))
            {
                XmlDocument db = new XmlDocument();
                db.Load(options.MS);

                //string date = DateTime.Now.ToString("yyyy-dd-MM");
                XmlNodeList rows = db.SelectNodes($"//row[TIME_STAMP]");

                foreach (XmlNode row in rows)
                {
                    Load load = new Load(ID++, DateTime.Parse(row.SelectSingleNode("TIME_STAMP").InnerText), double.Parse(row.SelectSingleNode("MEASURED_VALUE").InnerText), double.Parse(row.SelectSingleNode("FORECAST_VALUE").InnerText));

                    loads.Add(load);
                }

                options.Dispose();
            }

            return loads;
        }

        public void Write(List<Load> loads, List<Audit> audits, List<ImportedFile> files, string loadsPath, string auditsPath, string filesPath)
        {

            WriteLoad(loads, loadsPath);
            WriteAudit(audits, auditsPath);
            WriteImportedFile(files, filesPath);

        }

        private void WriteAudit(List<Audit> audits, string path)
        {
            using (FileManipulationOptions options = OpenFile(path))
            {
                XmlDocument db = new XmlDocument();
                db.Load(options.MS);

                XmlNodeList rows = db.SelectNodes("//STAVKA");
                int maxID = rows.Count;

                foreach (Audit a in audits)
                {
                    a.Id = ++maxID;

                    XmlElement newRow = db.CreateElement("STAVKA");

                    XmlElement idElement = db.CreateElement("ID");
                    idElement.InnerText = a.Id.ToString();

                    XmlElement timeStampElement = db.CreateElement("TIME_STAMP");
                    timeStampElement.InnerText = a.Timestamp.ToString("yyyy-MM-dd HH:mm");

                    XmlElement messageTypeElement = db.CreateElement("MESSAGE_TYPE");
                    messageTypeElement.InnerText = a.Type.ToString();

                    XmlElement messageElement = db.CreateElement("MESSAGE");
                    messageElement.InnerText = a.Message;

                    newRow.AppendChild(idElement);
                    newRow.AppendChild(timeStampElement);
                    newRow.AppendChild(messageTypeElement);
                    newRow.AppendChild(messageElement);

                    XmlElement rootElement = db.DocumentElement;
                    rootElement.AppendChild(newRow);
                    db.Save(path);
                }

                if (audits.Count == 0)
                {
                    XmlElement newRow = db.CreateElement("STAVKA");

                    XmlElement idElement = db.CreateElement("ID");
                    idElement.InnerText = (++maxID).ToString();

                    XmlElement timeStampElement = db.CreateElement("TIME_STAMP");
                    timeStampElement.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

                    XmlElement messageTypeElement = db.CreateElement("MESSAGE_TYPE");
                    messageTypeElement.InnerText = "Info";

                    XmlElement messageElement = db.CreateElement("MESSAGE");
                    messageElement.InnerText = "Data successfully entered into database";

                    newRow.AppendChild(idElement);
                    newRow.AppendChild(timeStampElement);
                    newRow.AppendChild(messageTypeElement);
                    newRow.AppendChild(messageElement);

                    XmlElement rootElement = db.DocumentElement;
                    rootElement.AppendChild(newRow);
                    db.Save(path);
                }

                options.Dispose();
            }
        }

        private void WriteImportedFile(List<ImportedFile> files, string path)
        {
            using (FileManipulationOptions options = OpenFile(path))
            {
                XmlDocument db = new XmlDocument();
                db.Load(options.MS);

                XmlNodeList rows = db.SelectNodes("//row");
                int maxID = rows.Count;

                foreach (ImportedFile a in files)
                {
                    a.Id = ++maxID;

                    XmlElement newRow = db.CreateElement("row");

                    XmlElement idElement = db.CreateElement("ID");
                    idElement.InnerText = a.Id.ToString();

                    XmlElement nameElement = db.CreateElement("FILE_NAME");
                    nameElement.InnerText = a.FileName;

                    newRow.AppendChild(idElement);
                    newRow.AppendChild(nameElement);

                    XmlElement rootElement = db.DocumentElement;
                    rootElement.AppendChild(newRow);
                    db.Save(path);
                }

                if (files.Count == 0)
                {
                    XmlElement newRow = db.CreateElement("row");

                    XmlElement idElement = db.CreateElement("ID");
                    idElement.InnerText = (++maxID).ToString();

                    XmlElement nameElement = db.CreateElement("FILE_NAME");
                    nameElement.InnerText = "FILENAME";

                    newRow.AppendChild(idElement);
                    newRow.AppendChild(nameElement);

                    XmlElement rootElement = db.DocumentElement;
                    rootElement.AppendChild(newRow);
                    db.Save(path);
                }

                options.Dispose();
            }
        }

        private void WriteLoad(List<Load> podaci, string path)
        {
            using (FileManipulationOptions options = OpenFile(path))
            {
                XmlDocument db = new XmlDocument();
                db.Load(options.MS);

                options.MS.Position = 0;

                XmlNodeList rows = db.SelectNodes("//row");
                int maxID = rows.Count;

                foreach (Load l in podaci)
                {
                    XmlNode element = null;

                    try
                    {
                        element = db.SelectSingleNode($"//row[TIME_STAMP = '{l.Timestamp.ToString("yyyy-MM-dd HH:mm")}']");
                    }
                    catch { }


                    if (element != null)
                    {
                        if(l.ForecastValue == -1)
                        {
                            element.SelectSingleNode("MEASURED_VALUE").InnerText = l.MeasuredValue.ToString();
                            db.Save(path);
                        }
                        else
                        {
                            element.SelectSingleNode("FORECAST_VALUE").InnerText = l.ForecastValue.ToString(CultureInfo.InvariantCulture);
                            db.Save(path);
                        }
                    }
                    else
                    {
                        XmlElement newRow = db.CreateElement("row");


                        XmlElement timeStampElement = db.CreateElement("TIME_STAMP");
                        timeStampElement.InnerText = l.Timestamp.ToString("yyyy-MM-dd HH:mm");

                        XmlElement measuredValueElement = db.CreateElement("MEASURED_VALUE");
                        measuredValueElement.InnerText = l.MeasuredValue.ToString();
                        
                        XmlElement forecastValueElement = db.CreateElement("FORECAST_VALUE");
                        forecastValueElement.InnerText = l.ForecastValue.ToString(CultureInfo.InvariantCulture);
                        
                        XmlElement absDevElement = db.CreateElement("ABSOLUTE_PERCENTAGE_DEVIATION");
                        absDevElement.InnerText = l.AbsolutePercentageDeviation.ToString();
                        
                        XmlElement squaredDevElement = db.CreateElement("SQUARED_DEVIATION");
                        squaredDevElement.InnerText = l.SquaredDeviation.ToString();

                        XmlElement idElement = db.CreateElement("IMPORTED_FILE_ID");
                        idElement.InnerText = l.ImportedFileId.ToString();

                        newRow.AppendChild(timeStampElement);
                        newRow.AppendChild(measuredValueElement);
                        newRow.AppendChild(forecastValueElement);
                        newRow.AppendChild(absDevElement);
                        newRow.AppendChild(squaredDevElement);
                        newRow.AppendChild(idElement);

                        XmlElement rootElement = db.DocumentElement;
                        rootElement.AppendChild(newRow);
                        db.Save(path);
                    }
                }

                options.Dispose();
            }
        }

        public void WriteCalculation(List<Load> podaci, string path)
        {
            using (FileManipulationOptions options = OpenFile(path))
            {
                XmlDocument db = new XmlDocument();
                db.Load(options.MS);

                options.MS.Position = 0;

                XmlNodeList rows = db.SelectNodes("//row");
                int maxID = rows.Count;

                foreach (Load l in podaci)
                {
                    XmlNode element = null;

                    try
                    {
                        element = db.SelectSingleNode($"//row[TIME_STAMP = '{l.Timestamp.ToString("yyyy-MM-dd HH:mm")}']");
                    }
                    catch { }


                    if (element != null)
                    {
                       
                            element.SelectSingleNode("ABSOLUTE_PERCENTAGE_DEVIATION").InnerText = l.AbsolutePercentageDeviation.ToString();
                            element.SelectSingleNode("SQUARED_DEVIATION").InnerText = l.SquaredDeviation.ToString();
                            db.Save(path);
                       
                    }
                    
                }

                options.Dispose();
            }
        }

        public FileManipulationOptions OpenFile(string path)
        {
            if (!File.Exists(path))
            {
                string start = "";
                if (path.ToLower().Contains("audit"))
                    start = "STAVKE";
                else
                    start = "rows";

                XDocument xml = new XDocument(new XElement(start));
                xml.Save(path);
            }

            MemoryStream stream = new MemoryStream();

            using (FileStream xml = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                xml.CopyTo(stream);
                xml.Dispose();
            }

            stream.Position = 0;

            return new FileManipulationOptions(stream, Path.GetFileName(path));
        }
    }
}
