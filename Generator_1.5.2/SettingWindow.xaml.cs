using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace Generator_1._5._2
{
   
    public partial class SettingWindow : MetroWindow
    {
        public SettingWindow()
        {
            InitializeComponent();
        }
        
        private void RefreshBtn_OnClick(object sender, RoutedEventArgs e)
        {
          MainWindow.Instance.Refresh();
        }

        private void RangeBase_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MainWindow.Instance == null) return;
            MainWindow.Instance.SetProfitLevell((int)e.NewValue);
        }
    }
}
