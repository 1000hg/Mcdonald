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

        private List<SeatItemCtrl> seatItems = new List<SeatItemCtrl>();

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
            LoadSeat();
        }

        private void Ctrl_OnOrderComplete(object sender, OrderArgs args)
        {
            string msg = "Table " + args.seatIdx + " Order completed";
            MessageBox.Show(msg);
            OrderControl.Visibility = Visibility.Hidden;
            lvSeat.Visibility = Visibility.Visible;
            UpdateSeat();
        }

        private void Ctrl_OnPaymentComplete(object sender, OrderArgs args)
        {
            string msg = "Table " + args.seatIdx + "Payment completed";
            MessageBox.Show(msg);
            OrderControl.Visibility = Visibility.Hidden;
            lvSeat.Visibility = Visibility.Visible;
            UpdateSeat();
        }

        private void LvSeat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lvSeat.SelectedIndex == -1)
            {
                return;
            }

            OrderControl.SetSeatIdx((lvSeat.SelectedItem as SeatItemCtrl).GetSeat().Idx);
            OrderControl.Visibility = Visibility.Visible;
            lvSeat.Visibility = Visibility.Hidden;
        }

        private void UpdateSeat()
        {
            LoadSeat();
            lvSeat.Items.Refresh();
        }

        private void LoadSeat()
        {
            seatItems.Clear();

            foreach (Seat seat in App.SeatData.lstSeat)
            {
                SeatItemCtrl seatItemCtrl = new SeatItemCtrl();
                seatItemCtrl.SetSeat(seat);

                seatItems.Add(seatItemCtrl);
            }

            lvSeat.ItemsSource = seatItems;
        }
    }
}
