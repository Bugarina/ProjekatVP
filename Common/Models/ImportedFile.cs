using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class ImportedFile
    {
        private int id;
        private string fileName;

        public int Id { get => id; set => id = value; }
        public string FileName { get => fileName; set => fileName = value; }

        public ImportedFile(int id, string filename)
        {
            this.id = id;
            this.fileName = filename;
        }

        public ImportedFile()
        {

        }
    }
}
