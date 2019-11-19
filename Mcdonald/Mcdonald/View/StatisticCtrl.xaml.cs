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
    /// <summary>
    /// Interaction logic for StatisticCtrl.xaml
    /// </summary>
    /// 


    public partial class StatisticCtrl : UserControl
    {
        private CategoryManager categoryManager = new CategoryManager();

        private List<Food> foods = new List<Food>();




        public StatisticCtrl()
        {
            InitializeComponent();

            this.Loaded += StatisticCtrl_Loaded;
        }

        private void StatisticCtrl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = App.FoodData;

            App.CategoryData.Load();
            lvCategory.ItemsSource = App.CategoryData.lstCategory;

            lvCategory.SelectedIndex = 0;

        }



        private void LvCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lvCategory.SelectedIndex == -1)
                return;

            Category category = (lvCategory.SelectedItem as Category);
            if (category == null) return;
            updateFood(category);
            categoryTotalText.Text = "총합 : " + findCategoryTotal();
        }

        public void updateFood(Category category)
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
            foodChart.ItemsSource = foods;
            foodChart.Items.Refresh();
        }

        private bool checkCategory(Food food, Category category)
        {
            return food.Category.Equals(categoryManager.getEnum(category.Title)) || categoryManager.getEnum(category.Title) == eCategory.None;
        }


        public string findCategoryTotal()
        {
            int CategoryTotal = 0;

            Category category = (lvCategory.SelectedItem as Category);

            foreach (Food food in App.FoodData.lstFood)
            {
                {
                    bool isSameCategory = checkCategory(food, category);
                    if (isSameCategory)
                    {
                        CategoryTotal += food.StatisticTotal;
                    }
                }
            }
            categoryTotalText.Text = "총합 : " + CategoryTotal.ToString();
            Debug.WriteLine("총합 : " + CategoryTotal.ToString());
            return CategoryTotal.ToString();

        }


        private void changeTotal(object sender, RoutedEventArgs e)
        {
            if (graph.Visibility == Visibility.Visible)
            {
                graph.Visibility = Visibility.Hidden;
                time.Visibility = Visibility.Visible;
                totalChange.Content = "그래프";
            }
            else
            {
                graph.Visibility = Visibility.Visible;
                time.Visibility = Visibility.Hidden;
                totalChange.Content = "시간별";
            }
        }

        public void UpdateDayTotal()
        {
            foods.Clear();

            foreach(Statistics statistics in App.StatisticsData.lstStatistics)
            {
                foreach(Food food in statistics.FoodList)
                {
                    food.Date = statistics.Date;
                    foods.Add(food);
                }

            }

            dayTotal.ItemsSource = foods;
            dayTotal.Items.Refresh();
        }

    }
}
