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

        //public delegate void connectCollback(IAsyncResult ar);
        //public event connectCollback connectCollback;

        DispatcherTimer myTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            myTimer.Interval = new TimeSpan(0, 0, 1);
            myTimer.Tick += myTimer_Tick;
            myTimer.Start();
            this.Loaded += MainWindow_Loaded;
            App.client.OnConnectComplete += Ctrl_OnConnectComplete;
            App.client.OnConnectFail += Ctrl_OnConnectFail;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            App.client.ConnectServer();

            /*if (!App.client.IsConnected)
            {
               // App.client.SendMessage("@2207");
               // Debug.WriteLine("연결됨");
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("서버가 연결되지 않았습니다. 다시 연결할까요? ", "Reload", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        App.client.ConnectServer();
                      if (App.client.IsConnected)
                      {
                            App.client.SendMessage("@2207");
                      }
                      break;
                    case MessageBoxResult.No:
                        return;
                }
                //Debug.WriteLine("연결되지 않음");
            }*/
        }

        private void Ctrl_OnConnectComplete(object sender)
        {
            App.client.SendMessage("@2207");
            Debug.WriteLine("연결됨");
        }

        private void Ctrl_OnConnectFail(object sender)
        {
            MessageBoxResult result = MessageBox.Show("서버가 연결되지 않았습니다. 다시 연결할까요? ", "Reload", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        App.client.ConnectServer();
                      break;
                    case MessageBoxResult.No:
                        MessageBox.Show("서버에 연결이 안된 채 수행됩니다.");
                        return;
                }
        }

        void myTimer_Tick(object sender, EventArgs e)
        {
            txtTime.Text = DateTime.Now.ToString();
        }

        private void Logo_Click(object sender, MouseButtonEventArgs e)
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

            Category category = App.CategoryData.lstCategory.Find(x => x.Title == "All Menu");
            StatisticControl.updateFood(category);
            StatisticControl.categoryTotalText.Text = "총합 : " + StatisticControl.findCategoryTotal();
            StatisticControl.UpdateDayTotal();

            /*if (App.client.IsConnected)
            {
                App.client.SendMessage("@All#" + StatisticControl.findCategoryTotal());
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("서버가 연결되지 않았습니다. 다시 연결할까요? ", "Reload", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        App.client.ConnectServer();
                        if (App.client.IsConnected)
                        {
                            App.client.SendMessage("@2207");
                        }
                        break;
                    case MessageBoxResult.No:
                        return;
                }
            }*/
            App.client.SendMessage("@All#" + StatisticControl.findCategoryTotal());

        }
    }
}
