using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcer.SMA
{
    public class SmaBuilder
    {
        public SmaBuilder(int period)
        {
            Period = period;
            _basicList = new List<decimal>();
        }

        public void ProcessSma(decimal close)
        {
            _basicList.Add(close);

            if (_basicList.Count <= Period - 1) return;

            OnNewSmaValue(decimal.Round((decimal)(_basicList.Sum() / Period), 0));
            _basicList.RemoveAt(0);

        }

        public event Action<decimal> NewSmaValue;

        protected virtual void OnNewSmaValue(decimal obj)
        {
            Action<decimal> handler = NewSmaValue;
            if (handler != null) handler(obj);
        }

        private List<decimal> _basicList;

        public int Period { get; private set; }

        public void Clearn()
        {
            _basicList.Clear();
        }

    }
}
