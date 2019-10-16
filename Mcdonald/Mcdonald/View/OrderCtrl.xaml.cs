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
            lvCategory.ItemsSource = App.CategoryData.lstCategory;
        }

        public void SetSeatIdx(int idx)
        {
            seat = App.SeatData.lstSeat[idx - 1];

            lvSelected.ItemsSource = seat.FoodList;

            UpdateSelectedFood();
            SetFoodList();

            SeatIdx.Text = "Table " + idx.ToString();
        }

        private void SetFoodList()
        {
            List<Food> clone = new List<Food>(App.FoodData.lstFood.Count);

            App.FoodData.lstFood.ForEach((food) =>
            {
                seat.FoodList.ForEach((selected) =>
                {
                    if (food.Name == selected.Name)
                    {
                        food.Count = selected.Count;
                    }
                });
                clone.Add(new Food(food));
            });

            lvFood.ItemsSource = clone;
        }

        private void LvCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Category category = (lvCategory.SelectedItem as Category);
            if (category == null) return;
            UpdateFood(category);
        }

        private void UpdateFood(Category category)
        {
            foods.Clear();
            foreach (Food food in App.FoodData.lstFood)
            {
                bool isSameCategory = CheckCategory(food, category);
                if (isSameCategory)
                {
                    foods.Add(food);
                }
            }
            lvFood.ItemsSource = foods;
            lvFood.Items.Refresh();
        }

        private bool CheckCategory(Food food, Category category)
        {
            return food.Category.Equals(categoryManager.getEnum(category.Title)) || categoryManager.getEnum(category.Title) == eCategory.None;
        }

        private void BtnPlus_Click(object sender, RoutedEventArgs e)
        {
            Button button = (sender as Button);
            Food food = button.DataContext as Food;
            if (food == null) return;
            PlusFood(food);
            lvFood.Items.Refresh();
        }

        private void PlusFood(Food food)
        {
            food.Count++;
            if (food.Count == 1)
            {
                seat.FoodList.Add(food);
            }
            UpdateSelectedFood();
        }

        private void BtnMinus_Click(object sender, RoutedEventArgs e)
        {
            Button button = (sender as Button);
            Food food = button.DataContext as Food;
            if (food == null) return;
            MinusFood(food);
            lvFood.Items.Refresh();
        }

        private void MinusFood(Food food)
        {
            food.Count--;
            if (food.Count == 0)
            {
                seat.FoodList.Remove(food);
            }
            UpdateSelectedFood();
        }

        private void UpdateSelectedFood()
        {
            //lvSelected.Items.Clear();
            TotalPrice.Text = seat.TotalPrice.ToString();
            lvSelected.Items.Refresh();
        }

        private void PaymentBtn_Click(object sender, RoutedEventArgs e)
        {
            InsertStatisticsData();
            DeleteSeatData();

            OrderArgs args = new OrderArgs();
            args.seatIdx = seat.Idx;

            if (OnPaymentComplete != null)
            {
                OnPaymentComplete(this, args);
            }
        }

        private void OrderBtn_Click(object sender, RoutedEventArgs e)
        {
            InsertSeatData();

            OrderArgs args = new OrderArgs();
            args.seatIdx = seat.Idx;

            if (OnOrderComplete != null)
            {
                OnOrderComplete(this, args);
            }
        }

        private void InsertStatisticsData()
        {
            App.StatisticsData.lstStatistics.Add(new Statistics { Date = new DateTime(), FoodList = seat.FoodList });
        }

        private void InsertSeatData()
        {
            App.SeatData.lstSeat
                .Where(w => w.Idx == seat.Idx).ToList()
                .ForEach(s =>
                {
                    int position = App.SeatData.lstSeat.IndexOf(s);
                    App.SeatData.lstSeat[position] = seat;
                });
        }

        private void DeleteSeatData()
        {
            App.SeatData.lstSeat
                .Where(w => w.Idx == seat.Idx).ToList()
                .ForEach(s => App.SeatData.lstSeat.Remove(s));
        }
    }
}
