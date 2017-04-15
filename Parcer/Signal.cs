using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parcer.Candles;
using StockSharp.BusinessEntities;

namespace Parcer
{
    public class Signal
    {
        public double Sma { get; set; }
        public TimeCandle M5Candle { get; set; }
        public TimeCandle M1Candle { get; set; }
        public TickCandle TickCandle { get; set; }
        public Trade Trade { get; set; }
    }
}
