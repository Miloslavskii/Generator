using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Generator;
using LiveCharts;
using LiveCharts.Wpf;
using Ecng.Xaml;
using MahApps.Metro.Controls;
using Parcer;
using StockSharp.BusinessEntities;

namespace Generator_1._5._2
{

    public partial class MainWindow : MetroWindow
    {
        private SettingWindow _settingWindow = new SettingWindow();

        private readonly List<Day> _days = new List<Day>();
        private Aggregator _aggregator = new Aggregator();
        private List<List<Signal>> _signals = new List<List<Signal>>();
        private Gen _gen = new Gen();

        public SeriesCollection SeriesCollection { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Pnl",
                    Values = new ChartValues<double>()
                },
            };
            DataContext = this;
            _settingWindow.MakeHideable();
            InitChart();
            Instance = this;
        }

        public static MainWindow Instance;

        private  async void LoadBtn_Click(object sender, RoutedEventArgs e)
        {
            TickParce.StartParcing += () => Dispatcher.Invoke(() =>
            {
                ProgressRing.IsActive = true;
                MetroWnd.GlowBrush = new SolidColorBrush(Colors.OrangeRed);
            });
           
            TickParce.EndParcing+= () => Dispatcher.Invoke(() =>
            {
                ProgressRing.IsActive = false;
                MetroWnd.GlowBrush = new SolidColorBrush(Colors.SkyBlue);
                LoadBtn.Background = new SolidColorBrush(Colors.SkyBlue);
            });
            
            var trades = await Task<List<Trade>>.Factory.StartNew(TickParce.Start);
            var day = new Day().BuildDays(trades);
            _days.AddRange(day);
        }

        private async void LearnBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_aggregator.IsProcess) return;

            if (_days.Count == 0)
            {
                MessageBox.Show("_days.Count==0");
                return;
            }
            var count = 0;
            ProgressBar.Maximum = _days.Sum(day => day.Trades.Count);
            var scale = (decimal)ProgressBar.Maximum / 100;

            if (!_aggregator.IsStart)
            {
                _aggregator.SimTrader.NewTrade += trade =>
                {
                    count++;
                    if (count < scale) return;

                    Dispatcher.Invoke(new Action(() => ProgressBar.Value += count));
                    count = 0;
                };

                _aggregator.Finish += signals =>
                {
                    var newSignLst = new List<List<Signal>>(signals);
                    _signals.AddRange(newSignLst);
                    _aggregator.DaysSignals.Clear();

                    Dispatcher.Invoke(() =>
                    {
                        LoadBtn.Background = new SolidColorBrush(Colors.White);
                        ProgressBar.Value = 0;
                        _days.Clear();
                        Dqn();
                    });
                };
            }
            _aggregator.AddDays(_days);

            await Task.Factory.StartNew(_aggregator.ProcessDays);
        }

        private async void Dqn()
        {
            if (_signals.Count == 0)
            {
                MessageBox.Show("_signals.count == 0");
                return;
            }

            _gen.AddSignals(_signals);
            _aggregator.Normalization(_gen.Days);
            _signals.Clear();

            if (!_gen.IsStarted)
            {
                ComandSettingBtn.IsEnabled = true;
                await Task.Factory.StartNew(_gen.ProcessDays);
            }
                
        }

        public void Refresh()
        {
            _gen.Trader.Randomize();
        }

        public void SetProfitLevell(int prLevell)
        {
            _gen.Trader.ProfitLevell = prLevell;
        }


        private void InitChart()
        {
            var count = 0;

            _gen.Portfolio.FullTrades += s => Dispatcher.Invoke(() =>
            {
                count++;
                if (count < 100) return;

                SeriesCollection[0].Values.Add((double)s);

                count = 0;
            });
        }


        private void ComandSettingBtn_OnClick(object sender, RoutedEventArgs e)
        {
            ShowOrHide(_settingWindow);
        }

        private void ShowOrHide(Window window)
        {
            if (window.Visibility == Visibility.Visible)
                Dispatcher.Invoke(window.Hide);
            else
                Dispatcher.Invoke(window.Show);
        } 


        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
            base.OnClosing(e);
        }
    }
}
