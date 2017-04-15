using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parcer.Candles;
using StockSharp.BusinessEntities;

namespace Parcer
{
    public class SimTrader
    {
        public List<Day> Days { get; set; }
        public TimeCandleBuilder M5Builder { get; set; }
        public TimeCandleBuilder M1Builder { get; set; }
        public TickCandleBuilder TickCandleBuilder { get; set; }

        private int _count = 0;

        public SimTrader(List<Day> days)
        {
            Days = days;
            TickCandleBuilder = new TickCandleBuilder(this, 150);
            M1Builder = new TimeCandleBuilder(this, TimeSpan.FromMinutes(1));
            M5Builder = new TimeCandleBuilder(this, TimeSpan.FromMinutes(5));
        }

        /// <summary>
        /// Событие прихода нового трейда
        /// </summary>
        public event Action<Trade> NewTrade;
        /// <summary>
        /// Событие окончания трейдов за день
        /// </summary>
        public event Action DayEnd;
        /// <summary>
        /// Событие окончания дней в выборке
        /// </summary>
        public event Action Finish;

        protected virtual void OnFinish()
        {
            var handler = Finish;
            if (handler != null) handler();
        }

        protected virtual void OnDayEnd()
        {
            var handler = DayEnd;
            if (handler != null) handler();
        }

        protected virtual void OnNewTrade(Trade trade)
        {
            var handler = NewTrade;
            if (handler != null) handler(trade);
        }

        /// <summary>
        /// Начать получать трейды за следующий день
        /// </summary>
        public void NextDay()
        {
            if (!TickCandleBuilder.IsStarted || !M5Builder.IsStarted || !M1Builder.IsStarted)
            {
                TickCandleBuilder.StartSim();
                M5Builder.StartSim();
                M1Builder.StartSim();
            }

            if (_count >= Days.Count)
            {
                OnFinish();
                _count = 0;
                End();
                return;
            }

            foreach (var trade in Days[_count].Trades)
            {
                OnNewTrade(trade);
            }

            _count++;

            Clean();
            OnDayEnd();

        }

        private void Clean()
        {
            M5Builder.Clean();
            M1Builder.Clean();
            TickCandleBuilder.Clean();
        }

        private void End()
        {
            Clean();
            Days.Clear();
        }

    }
}
