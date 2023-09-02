using Common.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Server.FileTransportService;

namespace Server
{
    public class Calculation
    {
        public event CalculationDelegateHandler absEvent;
        public event CalculationDelegateHandler squareEvent;

        public Calculation()
        {
            absEvent += Calculation_absEvent;
            squareEvent += Calculation_squareEvent;
        }

        private List<Load> Calculation_squareEvent(object sender, List<Load> args)
        {
            foreach (Load item in args)
            {
                if(item.MeasuredValue != -1 && item.ForecastValue != -1)
                item.SquaredDeviation = Math.Pow(((item.MeasuredValue - item.ForecastValue) / item.ForecastValue),2);
            }
            return args;
        }

        private List<Load> Calculation_absEvent(object sender, List<Load> args)
        {
            foreach (Load item in args)
            {
                if (item.MeasuredValue != -1 && item.ForecastValue != -1)
                    item.AbsolutePercentageDeviation = ((item.MeasuredValue - item.ForecastValue) / item.ForecastValue) * 100;
            }
            return args;
        }

        public List<Load> InvokeEvent(List<Load> data)
        {
            List<Load> results = new List<Load>();
            if(ConfigurationManager.AppSettings["CalcType"] == "absolute")
            {

            results = absEvent.Invoke(this, data);
            }
            else
            {
                results = squareEvent.Invoke(this,data);
            }
            return results;
        }

    }
}
