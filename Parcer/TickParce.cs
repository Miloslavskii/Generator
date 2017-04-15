using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;
using StockSharp.BusinessEntities;

namespace Parcer
{
    public class TickParce
    {
        public static event Action StartParcing;

        protected  static void OnStartParcing()
        {
            Action handler = StartParcing;
            if (handler != null) handler();
        }

        public static List<Trade> Start()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            var tradeLst = new List<Trade>();
            var dialog = new OpenFileDialog { Title = "Открыть файл тиковой истории" };
            
            if (dialog.ShowDialog() == true)
            {
                using (var openFile = dialog.OpenFile())
                {
                    using (var streamReader = new StreamReader(openFile))
                    {
                        OnStartParcing();

                        // Пропустить строку с легендой
                        streamReader.ReadLine();

                        while (!streamReader.EndOfStream)
                        {
                            var line = streamReader.ReadLine();
                            string[] items = line.Split(',');
                            var newString = items[2] + items[3];

                            var trade = new Trade
                            {
                                Time = DateTime.ParseExact(newString, "yyyyMMddHHmmss", null),
                                Price = decimal.Parse(items[4]), // decimal.Round(decimal.Parse(items[4]),5),
                                Volume = int.Parse(items[5]),
                            };
                            tradeLst.Add(trade);
                        }
                    }
                }
            }
            return tradeLst;
        }

    }
}
