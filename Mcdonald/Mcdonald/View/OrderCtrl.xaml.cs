using LbMcdonald.Model;
using Mcdonald.Manager;
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

    public class OrderArgs : EventArgs
    {
        public int seatIdx { get; set; }
    }

    public partial class OrderCtrl : UserControl
    {
        public delegate void OrederCompleteHandler(object sender, OrderArgs args);
        public event OrederCompleteHandler OnOrderComplete;

        public delegate void PaymentCompleteHandler(object sender, OrderArgs args);
        public event PaymentCompleteHandler OnPaymentComplete;

        private CategoryManager categoryManager = new CategoryManager();

        private List<Food> foods = new List<Food>();

        private Seat seat = new Seat();

        public OrderCtrl()
        {
            InitializeComponent();
            this.Loaded += OrderCtrl_Loaded;
        }

        private void OrderCtrl_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Order_load");
            App.CategoryData.Load();
            App.FoodData.Load();
            lvCategory.ItemsSource = App.CategoryData.lstCategory;
            lvFood.ItemsSource = App.FoodData.lstFood;
        }

        public void setSeatIdx(int idx)
        {
            seat.Idx = idx;
            SeatIdx.Text = "Table " + idx.ToString();
        }

        private void LvCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Category category = (lvCategory.SelectedItem as Category);
            if (category == null) return;
            updateFood(category);
        }

        private void updateFood(Category category)
        {
            foods.Clear();
            foreach (Food food in App.FoodData.lstFood)
            {
                bool isSameCategory = checkCategory(food, category);
                if (isSameCategory)
                {
                    foods.Add(food);
                }
            }
            lvFood.ItemsSource = foods;
            lvFood.Items.Refresh();
        }

        private bool checkCategory(Food food, Category category)
        {
            return food.Category.Equals(categoryManager.getEnum(category.Title)) || categoryManager.getEnum(category.Title) == eCategory.None;
        }

        private void BtnPlus_Click(object sender, RoutedEventArgs e)
        {
            Button button = (sender as Button);
            Food food = button.DataContext as Food;
            if (food == null) return;
            plusFood(food);
            lvFood.Items.Refresh();
        }

        private void plusFood(Food food)
        {
            food.Count++;
            if (food.Count == 1)
            {
                seat.FoodList.Add(food);
            }
            updateSelectedFood();
        }

        private void BtnMinus_Click(object sender, RoutedEventArgs e)
        {
            Button button = (sender as Button);
            Food food = button.DataContext as Food;
            if (food == null) return;
            minusFood(food);
            lvFood.Items.Refresh();
        }

        private void minusFood(Food food)
        {
            food.Count--;
            if (food.Count == 0)
            {
                seat.FoodList.Remove(food);
            }
            updateSelectedFood();
        }

        private void updateSelectedFood()
        {
            lvSelected.ItemsSource = seat.FoodList;
            TotalPrice.Text = seat.TotalPrice.ToString();
            lvSelected.Items.Refresh();
        }

        private void PaymentBtn_Click(object sender, RoutedEventArgs e)
        {
            deleteSeatData();

            OrderArgs args = new OrderArgs();
            args.seatIdx = seat.Idx;

            if (OnPaymentComplete != null)
            {
                OnPaymentComplete(this, args);
            }
        }

        private void OrderBtn_Click(object sender, RoutedEventArgs e)
        {
            insertSeatData();

            OrderArgs args = new OrderArgs();
            args.seatIdx = seat.Idx;

            if (OnOrderComplete != null)
            {
                OnOrderComplete(this, args);
            }
        }

        private void insertSeatData()
        {
            App.SeatData.lstSeat.Where(w => w.Idx == seat.Idx).ToList().ForEach(s => s = seat);
        }

        private void deleteSeatData()
        {
            App.SeatData.lstSeat.Where(w => w.Idx == seat.Idx).ToList().ForEach(s => App.SeatData.lstSeat.Remove(s));
        }
    }
}
