﻿using LbMcdonald.Model;
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
        public delegate void OrederBackHandler(object sender);
        public event OrederBackHandler OnOrderBack;

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
            seat = new Seat(App.SeatData.lstSeat[idx - 1]);

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
                seat.FoodList.RemoveAt(foodIdx);
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

        private void Back_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (OnOrderBack != null)
            {
                OnOrderBack(this);
            }
        }

        private void Clear_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            seat.FoodList.Clear();

            lvFood.ItemsSource = GetFoodList();

            UpdateSelectedFood();
        }

        private void PaymentBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CheckFoodEmpty()) return;

            MessageBoxResult result = MessageBox.Show("Total Price : " + seat.TotalPrice + "\nWould you like to pay? ", "Payment", MessageBoxButton.YesNoCancel);
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
            /*if (App.client.IsConnected)
            {
                App.client.SendMessage("@2207#" + seat.TotalPrice.ToString());
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("서버가 연결되지 않았습니다. 다시 연결할까요? ", "Reload", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        App.client.ConnectServer();
                        if(App.client.IsConnected)
                            App.client.SendMessage("@2207");
                      break;
                    case MessageBoxResult.No:
                        //this.Visibility = Visibility.Collapsed;
                        return;
                }
            }*/
            App.client.SendMessage("@2207#" + seat.TotalPrice.ToString());


            InsertStatisticsData();
            UpdateFoodDataTotalPrice();
            FindFoodRatio();
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

            foreach (Food food in App.FoodData.lstFood)
            {
                food.StatisticTotal = 0;
            }

            foreach (Statistics statistics in App.StatisticsData.lstStatistics)
            {
                foreach (Food payingFood in statistics.FoodList)
                {
                    foreach (Food food in App.FoodData.lstFood)
                    {
                        if (food.Name.Equals(payingFood.Name))
                        {
                            food.StatisticTotal += payingFood.Count * payingFood.Price;
                        }
                    }
                }
            }
        }

        private int FindMaxTotalPrice()
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

        private void FindFoodRatio()
        {
            int totalMaxPrice = FindMaxTotalPrice();

            foreach (Food food in App.FoodData.lstFood)
            {
                food.Ratio = ((float)food.StatisticTotal / totalMaxPrice * 350);
            }

        }

        private void OrderBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CheckFoodEmpty()) return;

            InsertSeatData();

            OrderArgs args = new OrderArgs();
            args.seatIdx = seat.Idx;

            if (OnOrderComplete != null)
            {
                OnOrderComplete(this, args);
            }
        }

        private bool CheckFoodEmpty()
        {
            if (seat.FoodList.Count == 0)
            {
                string msg = "Please Add Food";
                MessageBox.Show(msg);
                return true;
            }
            return false;
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
