using LbMcdonald.Model;
using Mcdonald.Manager;
using Mcdonald.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Mcdonald.View
{
    public partial class MainWindow : Window
    {
        DispatcherTimer myTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            App.CategoryData.Load();
            App.FoodData.Load();
            App.SeatData.Load();

            myTimer.Interval = new TimeSpan(0, 0, 1);
            myTimer.Tick += myTimer_Tick;
            myTimer.Start();
        }

        void myTimer_Tick(object sender, EventArgs e)
        {
            txtTime.Text = DateTime.Now.ToShortDateString();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Order_Click(object sender, RoutedEventArgs e)
        {
            SeatControl.Visibility = Visibility.Visible;
            StatisticControl.Visibility = Visibility.Hidden;
        }

        private void Statistic_Click(object sender, RoutedEventArgs e)
        {
            SeatControl.Visibility = Visibility.Hidden;
            StatisticControl.Visibility = Visibility.Visible;
        }
    }
}
