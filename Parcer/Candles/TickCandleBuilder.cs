using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockSharp.BusinessEntities;

namespace Parcer.Candles
{
    public class TickCandleBuilder
    {
        private SimTrader Trader { get; set; }

        private int TickCount { get; set; }
        private readonly List<Trade> _basicList = new List<Trade>();

        private TickCandle _tickCandle = new TickCandle();
        public bool IsStarted = false;

        public TickCandleBuilder(SimTrader trader, int tickCount)
        {
            Trader = trader;
            TickCount = tickCount;
        }

        public event Action<TickCandle> NewTickCandle;
        public event Action NewChange;

        protected virtual void OnNewChange()
        {
            Action handler = NewChange;
            if (handler != null) handler();
        }

        protected virtual void OnNewTickCandle(TickCandle candle)
        {
            var handler = NewTickCandle;
            if (handler != null) handler(candle);
        }

        public void StartSim()
        {
            IsStarted = true;

            Trader.DayEnd += Clean;

            Trader.NewTrade += trade =>
            {
                _basicList.Add(trade);

                if (_basicList.Count < TickCount + 1)
                {
                    return;
                }

                var delTr = _basicList.ElementAt(_basicList.Count - 1);

                var high = _basicList.Max(pr => pr.Price);
                var low = _basicList.Min(pr => pr.Price);

                var vol = _basicList.Sum(s => s.Volume);

                var open = _basicList.First().Price;
                var close = _basicList.Last().Price;
                var dir = open < close ? 1 : -1;

                var time = _basicList.Last().Time.DateTime;

                _tickCandle.Direction = dir;
                _tickCandle.Range = (double)(high - low);
                _tickCandle.Volume = (int)vol;
                _tickCandle.Close = close;

                OnNewTickCandle(_tickCandle);
                _basicList.Clear();
                _basicList.Add(delTr);
            };
        }

        public void Clean()
        {
            _basicList.Clear();
        }


    }
}
