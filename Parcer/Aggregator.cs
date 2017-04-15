using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parcer.Candles;
using Parcer.SMA;
using StockSharp.BusinessEntities;

namespace Parcer
{
    public class Aggregator
    {
        public SimTrader SimTrader { get; set; }
        private List<Day> Days = new List<Day>();

        public List<Signal> Signals;
        public List<List<Signal>> DaysSignals = new List<List<Signal>>();
        private TickCandle _tickCandle = new TickCandle();
        private TimeCandle _m1Candle = new TimeCandle();
        private TimeCandle _m5Candle = new TimeCandle();
        private Trade _trade = new Trade();
        private const int _period = 40;
        private decimal _sma = new decimal();

        private List<double> _m5VolLst = new List<double>();
        private List<double> _m5RangeLst = new List<double>();
        private List<double> _m1VolLst = new List<double>();
        private List<double> _m1RangeLst = new List<double>();
        private List<double> _tickVolLst = new List<double>();
        private List<double> _tickRangeLst = new List<double>();

        private SmaBuilder _smaBuilder = new SmaBuilder(_period);

        public bool IsStart = false;
        public bool IsProcess = false;

        public event Action<List<List<Signal>>> Finish;

        public Aggregator()
        {
            SimTrader = new SimTrader(this.Days);
            Signals = new List<Signal>();

            _smaBuilder.NewSmaValue += sma =>
            {
                _sma = sma;
            };

            SimTrader.NewTrade += trade =>
            {
                _trade = trade;
            };

            SimTrader.TickCandleBuilder.NewTickCandle += candle =>
            {
                _tickVolLst.Add(candle.Volume);
                _tickRangeLst.Add(candle.Range);
                _tickCandle = candle;
                Memorization();
            };

            SimTrader.M1Builder.NewTimeCandle += timeCandle =>
            {
                _m1VolLst.Add(timeCandle.Volume);
                _m1RangeLst.Add(timeCandle.Range);
                _m1Candle = timeCandle;
            };

            SimTrader.M5Builder.NewTimeCandle += timeCandle =>
            {
                _m5VolLst.Add(timeCandle.Volume);
                _m5RangeLst.Add(timeCandle.Range);

                _m5Candle = timeCandle;
                var closePrice = new decimal(timeCandle.Close);
                _smaBuilder.ProcessSma(closePrice);
            };

            SimTrader.DayEnd += () =>
            {
                var newLst = new List<Signal>(Signals);
                _smaBuilder.Clearn();
                DaysSignals.Add(newLst);
                Signals.Clear();
                SimTrader.NextDay();
            };

            SimTrader.Finish += () =>
            {
                Finish(DaysSignals);
                Signals.Clear();
                Days.Clear();
                IsProcess = false;
            };
        }

        public void ProcessDays()
        {
            IsProcess = true;
            IsStart = true;
            SimTrader.NextDay();
        }

        private void Memorization()
        {
            var m5Candle = new TimeCandle
            {
                Close = _m5Candle.Close,
                Direction = _m5Candle.Direction,
                Range = _m5Candle.Range,
                Volume = _m5Candle.Volume,
            };
            var m1Candle = new TimeCandle
            {
                Close = _m1Candle.Close,
                Direction = _m1Candle.Direction,
                Range = _m1Candle.Range,
                Volume = _m1Candle.Volume,
            };

            var tickCandle = new TickCandle
            {
                Close = _tickCandle.Close,
                Direction = _tickCandle.Direction,
                Range = _tickCandle.Range,
                Volume = _tickCandle.Volume,
            };

            var sma = new decimal((int)(tickCandle.Close - _sma));

            Signals.Add(new Signal
            {
                Sma = (double)(_sma == 0 ? 0 : sma),
                M5Candle = m5Candle,
                M1Candle = m1Candle,
                TickCandle = tickCandle,
                Trade = _trade,
            });

        }

        public void Normalization(List<List<Signal>> signals)
        {
            int volm5 = (int)Math.Floor(_m5VolLst.Count * 0.95);
            _m5VolLst.Sort();
            var nm5 = _m5VolLst[volm5];

            int rangM5 = (int)Math.Floor(_m5RangeLst.Count * 0.95);
            _m5RangeLst.Sort();
            var vR5 = _m5RangeLst[rangM5];

            int volm1 = (int)Math.Floor(_m1VolLst.Count * 0.95);
            _m1VolLst.Sort();
            var nm1 = _m1VolLst[volm1];

            int rangM1 = (int)Math.Floor(_m1RangeLst.Count * 0.95);
            _m1RangeLst.Sort();
            var vR1 = _m1RangeLst[rangM1];

            int volT = (int)Math.Floor(_tickVolLst.Count * 0.95);
            _tickVolLst.Sort();
            var nt150 = _tickVolLst[volT];

            int rangT150 = (int)Math.Floor(_tickRangeLst.Count * 0.95);
            _tickRangeLst.Sort();
            var vrT = _tickRangeLst[rangT150];

            var smaLst = (from daysSignal in signals from signal in daysSignal select signal.Sma).ToList();

            for (int index = 0; index < smaLst.Count; index++)
            {
                var s = smaLst[index];
                if (s == 0)
                {
                    smaLst.RemoveAt(index);
                    index--;
                }
                else if (s < 0)
                    smaLst[index] *= (-1);
            }
            int smaN = (int)Math.Floor(smaLst.Count * 0.95);
            smaLst.Sort();
            var sInd = smaLst[smaN];


            foreach (var signal1 in signals.SelectMany(signal => signal))
            {
                if (signal1.M5Candle.Volume > nm5)
                    signal1.M5Candle.Volume = 1.1;
                else
                    signal1.M5Candle.Volume = (double)decimal.Round((decimal)(signal1.M5Candle.Volume / nm5), 1);

                if (signal1.M5Candle.Range > vR5)
                    signal1.M5Candle.Range = 1.1;
                else
                    signal1.M5Candle.Range = (double)decimal.Round((decimal)(signal1.M5Candle.Range / vR5), 1);

                if (signal1.M1Candle.Volume > nm1)
                    signal1.M1Candle.Volume = 1.1;
                else
                    signal1.M1Candle.Volume = (double)decimal.Round((decimal)(signal1.M1Candle.Volume / nm1), 1);

                if (signal1.M1Candle.Range > vR1)
                    signal1.M1Candle.Range = 1.1;
                else
                    signal1.M1Candle.Range = (double)decimal.Round((decimal)(signal1.M1Candle.Range / vR1), 1);

                if (signal1.TickCandle.Volume > nt150)
                    signal1.TickCandle.Volume = 1.1;
                else
                    signal1.TickCandle.Volume = (double)decimal.Round((decimal)(signal1.TickCandle.Volume / nt150), 1);

                if (signal1.TickCandle.Range > vrT)
                    signal1.TickCandle.Range = 1.1;
                else
                    signal1.TickCandle.Range = (double)decimal.Round((decimal)(signal1.TickCandle.Range / vrT), 1);

                if (signal1.Sma > sInd)
                    signal1.Sma = 1.1;
                else
                    signal1.Sma = (double)decimal.Round((decimal)signal1.Sma / (decimal)sInd, 1);
            }
        }

        private void Clean()
        {
            _sma = 0;
            _m1Candle.Direction = 0;
            _m1Candle.Volume = 0;
            _m1Candle.Range = 0;
            _m1Candle.Close = 0;
            _m5Candle.Direction = 0;
            _m5Candle.Volume = 0;
            _m5Candle.Range = 0;
            _m5Candle.Close = 0;
            _tickCandle.Direction = 0;
            _tickCandle.Volume = 0;
            _tickCandle.Range = 0;
            _tickCandle.Close = 0;
            _trade.Price = 0;
        }

        public void AddDays(List<Day> days)
        {
            Days.AddRange(days);
        }
    }
}
