using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockSharp.BusinessEntities;

namespace Parcer.Candles
{
    public class TimeCandleBuilder
    {
        private SimTrader Trader { get; set; }
        private TimeSpan TimeFrame { get; set; }
      
        private readonly List<Trade> _basicList = new List<Trade>();

        private decimal _sumVolume = 0m;
        private decimal _maxPrice = 0m;
        private decimal _minPrice = 0m;
        private TimeSpan _timeRange;
        private TimeCandle _candle = new TimeCandle();

        public bool IsStarted = false;

        public TimeCandleBuilder(SimTrader trader, TimeSpan timeFrame)
        {
            Trader = trader;
            TimeFrame = timeFrame;
        }


        public event Action<TimeCandle> NewTimeCandle;

        public event Action NewChange;

        protected virtual void OnNewChange()
        {
            var handler = NewChange;
            if (handler != null) handler();
        }

        protected virtual void OnNewTimeCandle(TimeCandle candle)
        {
            var handler = NewTimeCandle;
            if (handler != null) handler(candle);
        }

        public void StartSim()
        {
            IsStarted = true;

            var periods = ExchangeBoard.Forts.WorkingTime.Periods;
            var startTime = periods[0].Times[0];
            var firstClearing = periods[0].Times[1];
            var secondClearing = periods[0].Times[2];
            _timeRange = startTime.Min + TimeFrame;

            Trader.DayEnd += Clean;

            Trader.NewTrade += trade =>
            {
                _basicList.Add(trade);

                if (_maxPrice < trade.Price) _maxPrice = trade.Price;
                if (_minPrice == 0) _minPrice = trade.Price;
                if (_minPrice > trade.Price) _minPrice = trade.Price;
                _sumVolume += trade.Volume;

                if (trade.Time.TimeOfDay < _timeRange) return;

                var delTr = _basicList.ElementAt(_basicList.Count - 1);

                _sumVolume -= trade.Volume;

                var range = _maxPrice - _minPrice;

                var open = _basicList.First().Price;
                var close = _basicList.Last().Price;
                var dir = open < close ? 1 : -1;
                var volume = _basicList.Sum(v => v.Volume);

                _candle.Direction = dir;
                _candle.Range = (double)range;
                _candle.Volume = (int)volume;
                _candle.Close = (double)close;

                OnNewTimeCandle(_candle);

                _maxPrice = 0;
                _minPrice = 0;
                _sumVolume = 0;

                _basicList.Clear();
                _basicList.Add(delTr);

                _sumVolume += delTr.Volume;

                if (_timeRange == startTime.Max)
                {
                    if (_basicList.First().Time.TimeOfDay > firstClearing.Min)
                    {
                        _timeRange = _basicList.First().Time.TimeOfDay;
                    }
                    else if (TimeFrame <= firstClearing.Min - startTime.Max)
                    {
                        _timeRange = firstClearing.Min;
                    }
                }
                else if (_timeRange == firstClearing.Max)
                {
                    if (_basicList.First().Time.TimeOfDay > secondClearing.Min)
                    {
                        _timeRange = _basicList.First().Time.TimeOfDay;
                    }
                    else if (TimeFrame <= secondClearing.Min - firstClearing.Max)
                    {
                        _timeRange = secondClearing.Min;
                    }
                }

                _timeRange += TimeFrame;
            };
        }

        public void Clean()
        {
            _basicList.Clear();
            _sumVolume = 0m;
            _maxPrice = 0m;
            _minPrice = 0m;
            var periods = ExchangeBoard.Forts.WorkingTime.Periods;
            var startTime = periods[0].Times[0];
            _timeRange = startTime.Min + TimeFrame;
        }
    }
}
