using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class FileManipulationOptions : IDisposable
    {

        public FileManipulationOptions(MemoryStream ms, string fileName)
        {
            this.MS = ms;
            this.FileName = fileName;
        }
        public FileManipulationOptions()
        {
            this.MS = new MemoryStream();
        }

        [DataMember]
        public MemoryStream MS { get; set; }

        [DataMember]
        public string FileName { get; set; }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    try
                    {
                        MS.Dispose();
                        MS.Close();
                        MS = null;
                    }
                    catch (System.Exception)
                    {
                        Console.WriteLine("Unsuccesful disposing!");
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
         ~FileManipulationOptions()
         {
             // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
             Dispose(disposing: false);
         }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
