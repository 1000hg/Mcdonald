using LbMcdonald.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mcdonald.View
{
    public partial class SeatItemCtrl : UserControl
    {
        Seat seat = new Seat();

        public SeatItemCtrl()
        {
            InitializeComponent();
        }

        public void SetSeat(Seat seat)
        {
            this.seat = seat;
            UpdateItem();
        }

        public Seat GetSeat()
        {
            return seat;
        }

        public void UpdateItem()
        {
            SeatIdx.Text = seat.Idx.ToString();
            lvFood.ItemsSource = seat.FoodList;
        }
    }
}
