using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public enum MessageType { Info, Warning, Error }
    public class Audit
    {

        private int id;
        private DateTime timestamp;
        private MessageType type;
        private string message;

        public int Id { get => id; set => id = value; }
        public DateTime Timestamp { get => timestamp; set => timestamp = value; }
        public MessageType Type { get => type; set => type = value; }
        public string Message { get => message; set => message = value; }

        public Audit(int id, DateTime timestamp, MessageType type, string message)
        {
            this.id = id;
            this.timestamp = timestamp;
            this.type = type;
            this.message = message;
        }
        public Audit()
        {

        }


    }
}
