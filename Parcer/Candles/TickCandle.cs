using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcer.Candles
{
    public class TickCandle
    {
        public decimal Close { get; set; }
        public double Range { get; set; }
        public double Volume { get; set; }
        public int Direction { get; set; }
    }
}
