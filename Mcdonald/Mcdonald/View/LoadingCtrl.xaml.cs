using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mcdonald.View
{
    public partial class LoadingCtrl : UserControl
    {
        public LoadingCtrl()
        {
            InitializeComponent();
            Loading();
        }

        private async void Loading()
        {
            App.CategoryData.Load();
            App.FoodData.Load();
            App.SeatData.Load();
            await Task.Delay(3000);
            this.Visibility = Visibility.Collapsed;
        }
    }
}
