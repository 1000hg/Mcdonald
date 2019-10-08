using LbMcdonald.Model;
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

namespace Mcdonald.View
{
    public class SeatArgs : EventArgs
    {
        public string seatId { get; set; }
    }

    public partial class SeatCtrl : UserControl
    {
        public SeatCtrl()
        {
            InitializeComponent();
            this.Loaded += SeatCtrl_Loaded;
            OrderControl.OnOrderComplete += Ctrl_OnOrderComplete;
            OrderControl.OnPaymentComplete += Ctrl_OnPaymentComplete;
        }

        private void SeatCtrl_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Seat_load");
            App.SeatData.Load();
            lvSeat.ItemsSource = App.SeatData.lstSeat;
        }

        private void Ctrl_OnOrderComplete(object sender, OrderArgs args)
        {
            string msg = "Table " + args.seatIdx + " Order completed";
            MessageBox.Show(msg);
            OrderControl.Visibility = Visibility.Hidden;
            lvSeat.Visibility = Visibility.Visible;
        }

        private void Ctrl_OnPaymentComplete(object sender, OrderArgs args)
        {
            string msg = "Table " + args.seatIdx + "Payment completed";
            MessageBox.Show(msg);
            OrderControl.Visibility = Visibility.Hidden;
            lvSeat.Visibility = Visibility.Visible;
        }

        private void LvSeat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OrderControl.setSeatIdx((lvSeat.SelectedItem as Seat).Idx);
            OrderControl.Visibility = Visibility.Visible;
            lvSeat.Visibility = Visibility.Hidden;
        }
    }
}
