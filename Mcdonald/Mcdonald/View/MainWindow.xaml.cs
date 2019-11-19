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

        Client client = new Client();

        public MainWindow()
        {
            InitializeComponent();

            myTimer.Interval = new TimeSpan(0, 0, 1);
            myTimer.Tick += myTimer_Tick;
            myTimer.Start();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (client.ConnectServer())
            {
                client.SendMessage("@2207");
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("서버가 연결되지 않았습니다. 다시 연결할까요? ", "Reload", MessageBoxButton.YesNoCancel);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                      if (client.ConnectServer())
                      {
                         client.SendMessage("@2207");
                      }
                      break;
                    case MessageBoxResult.No:
                        //종료
                        return;
                    case MessageBoxResult.Cancel:
                        //종료
                        return;
                }
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
            StatisticControl.findCategoryTotal();
            StatisticControl.UpdateDayTotal();

            if (client.IsConnected)
            {
                client.SendMessage("@All#" + StatisticControl.findCategoryTotal());
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("서버가 연결되지 않았습니다. 다시 연결할까요? ", "Reload", MessageBoxButton.YesNoCancel);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                      if (client.ConnectServer())
                      {
                         client.SendMessage("@2207");
                         client.SendMessage("@All#" + StatisticControl.findCategoryTotal());
                      }
                      break;
                    case MessageBoxResult.No:
                        //종료
                        return;
                    case MessageBoxResult.Cancel:
                        //종료
                        return;
                }
            }

        }
    }
}
