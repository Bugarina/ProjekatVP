using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Load
    {
        private int id;
        private DateTime timestamp;
        private double measuredValue;
        private double forecastValue;
        private double absolutePercentageDeviation;
        private double squaredDeviation;
        private int importedFileId;

        public int Id { get => id; set => id = value; }
        public DateTime Timestamp { get => timestamp; set => timestamp = value; }
        public double MeasuredValue { get => measuredValue; set => measuredValue = value; }
        public double ForecastValue { get => forecastValue; set => forecastValue = value; }
        public double AbsolutePercentageDeviation { get => absolutePercentageDeviation; set => absolutePercentageDeviation = value; }
        public double SquaredDeviation { get => squaredDeviation; set => squaredDeviation = value; }
        public int ImportedFileId { get => importedFileId; set => importedFileId = value; }

        public Load(int id, DateTime timestamp, double measuredValue, double forecastValue)
        {
            this.id = id;
            this.timestamp = timestamp;
            this.measuredValue = measuredValue;
            this.forecastValue = forecastValue;
        }

        public Load()
        {
        }
    }
}
