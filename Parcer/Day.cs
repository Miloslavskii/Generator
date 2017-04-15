using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockSharp.BusinessEntities;

namespace Parcer
{
    public class Day
    {
        public decimal High { get; private set; }
        public decimal Low { get; private set; }
        public decimal Open { get; private set; }
        public decimal Close { get; private set; }
        public decimal Range { get; private set; }
        public string Direction { get; private set; }
        public DateTime Date { get; private set; }
        public List<Trade> Trades { get; set; }

        public List<Day> BuildDays(List<Trade> trades)
        {
            var basicLst = new List<Trade>();
            var daysLst = new List<Day>();

            foreach (var trade in trades)
            {
                basicLst.Add(trade);
                if (basicLst.Count < 2) continue;

                var last = basicLst.ElementAt(basicLst.Count - 1);
                var prev = basicLst.ElementAt(basicLst.Count - 2);
                if (last.Time.Date == prev.Time.Date) continue;

                var delTr = basicLst.ElementAt(basicLst.Count - 1);
                basicLst.RemoveAt(basicLst.Count - 1);

                var open = basicLst.First().Price;
                var close = basicLst.Last().Price;
                var high = basicLst.Max(tr => tr.Price);
                var low = basicLst.Min(tr => tr.Price);
                var range = high - low;
                var median = decimal.Round((high + low) / 2, 0);
                var newTradeLst = new List<Trade>(basicLst);
                var date = basicLst.First().Time.Date;

                var day = new Day
                {
                    Open = open,
                    Close = close,
                    High = high,
                    Low = low,
                    Range = range,
                    Trades = newTradeLst,
                    Date = date,
                    Direction = close > open ? "Up" : "Down",
                };
                daysLst.Add(day);
                basicLst.Clear();
                basicLst.Add(delTr);
            }

            if (basicLst.Count == 0) return daysLst;


            var openLast = basicLst.First().Price;
            var closeLast = basicLst.Last().Price;
            var highLast = basicLst.Max(tr => tr.Price);
            var lowLast = basicLst.Min(tr => tr.Price);
            var rangeLast = highLast - lowLast;
            var medianLast = decimal.Round((highLast + lowLast) / 2, 0);
            var newTradeLstLast = new List<Trade>(basicLst);
            var dateLast = basicLst.First().Time.Date;

            var lastDay = new Day
            {
                Range = rangeLast,
                Trades = newTradeLstLast,
                Date = dateLast,
                Open = openLast,
                Close = closeLast,
                High = highLast,
                Low = lowLast,
                Direction = closeLast > openLast ? "Up" : "Down",
            };
            daysLst.Add(lastDay);
            basicLst.Clear();
            return daysLst;
        }
    }
}
