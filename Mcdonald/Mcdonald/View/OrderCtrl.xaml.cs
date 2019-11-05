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
            lvFood.ItemsSource = GetFoodList();

            UpdateSelectedFood();

            SeatIdx.Text = "Table " + idx.ToString();
        }

        private List<Food> GetFoodList()
        {
            List<Food> foods = new List<Food>();

            App.FoodData.lstFood.ForEach((food) =>
            {
                Food tempFood = new Food(food);

                seat.FoodList.ForEach((selected) =>
                {
                    if (food.Name == selected.Name)
                    {
                        tempFood.Count = selected.Count;
                    }
                });

                foods.Add(tempFood);
            });

            return foods;
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
            foreach (Food food in GetFoodList())
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
                Food tempFood = new Food(food);
                seat.FoodList.Add(tempFood);
            }
            else
            {
                int foodIdx = 0;

                seat.FoodList.ForEach((item) =>
                {
                    if (food.Name.Equals(item.Name))
                    {
                        foodIdx = seat.FoodList.IndexOf(item);
                    }
                });

                seat.FoodList[foodIdx].Count++;
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
            if (food.Count == 0) return;

            food.Count--;

            int foodIdx = 0;

            seat.FoodList.ForEach((item) =>
            {
                if (food.Name.Equals(item.Name))
                {
                    foodIdx = seat.FoodList.IndexOf(item);
                }
            });

            if (food.Count == 0)
            {
                try
                {
                    seat.FoodList.RemoveAt(foodIdx);
                }
                catch (Exception e)
                {
                    Debug.Print("Cannot be lowered to zero");
                }
            }
            else
            {
                seat.FoodList[foodIdx].Count--;
            }

            UpdateSelectedFood();
        }

        private void UpdateSelectedFood()
        {
            TotalPrice.Text = seat.TotalPrice.ToString();
            lvSelected.Items.Refresh();
        }

        private void PaymentBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Would you like to pay? ", "Payment", MessageBoxButton.YesNoCancel);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    PaymentEvent();
                    break;
                case MessageBoxResult.No:
                    return;
                case MessageBoxResult.Cancel:
                    return;
            }
        }

        private void PaymentEvent()
        {
            InsertStatisticsData();
            UpdateFoodDataTotalPrice();
            findFoodRatio();
            DeleteSeatData();

            OrderArgs args = new OrderArgs();
            args.seatIdx = seat.Idx;

            if (OnPaymentComplete != null)
            {
                OnPaymentComplete(this, args);
            }
        }

        private void UpdateFoodDataTotalPrice()
        {
            foreach (Statistics statistics in App.StatisticsData.lstStatistics)
            {
                foreach (Food payingFood in statistics.FoodList)
                {
                    foreach (Food food in App.FoodData.lstFood)
                    {
                        if (food.Name.Equals(payingFood.Name))
                        {
                            food.StatisticTotal = payingFood.Count * payingFood.Price;

                        }
                    }
                }
            }
        }

        private int findMaxTotalPrice()
        {
            int maxPrice = 0;
            foreach (Food food in App.FoodData.lstFood)
            {
                if (maxPrice < food.StatisticTotal)
                {
                    maxPrice = food.StatisticTotal;
                }
            }

            return maxPrice;
        }

        private void findFoodRatio()
        {
            int totalMaxPrice = findMaxTotalPrice();

            foreach (Food food in App.FoodData.lstFood)
            {
                food.Ratio = ((float)food.StatisticTotal / totalMaxPrice * 350);
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
            App.StatisticsData.lstStatistics.Add(new Statistics { Date = System.DateTime.Now, FoodList = seat.FoodList });
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
                .ForEach(s =>
                {
                    int position = App.SeatData.lstSeat.IndexOf(s);
                    App.SeatData.lstSeat[position] = new Seat() { Idx = s.Idx };
                });
        }
    }
}
