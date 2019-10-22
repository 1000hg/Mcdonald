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
    public partial class StatisticCtrl : UserControl
    {
        private CategoryManager categoryManager = new CategoryManager();

        private List<Food> foods = new List<Food>();

        List<Food> food = new List<Food>();

        public class CategoryTotal
        {
            public int Price { get; set; }

        }

        CategoryTotal categoryTotal = new CategoryTotal();



        public StatisticCtrl()
        {
            InitializeComponent();
            this.Loaded += StatisticCtrl_Loaded; ;
        }

        private void StatisticCtrl_Loaded(object sender, RoutedEventArgs e)
        {
            categoryTotalText.Text = "총합 :" + findCategoryTotal();
            this.DataContext = App.FoodData;

            App.FoodData.Load();
            foodChart.ItemsSource = App.FoodData.lstFood;

            App.CategoryData.Load();
            lvCategory.ItemsSource = App.CategoryData.lstCategory;
        }

        private void LvCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Category category = (lvCategory.SelectedItem as Category);
            if (category == null) return;
            updateFood(category);
            categoryTotalText.Text = "총합 : " + findCategoryTotal();
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
            foodChart.ItemsSource = foods;
            foodChart.Items.Refresh();
        }

        private bool checkCategory(Food food, Category category)
        {
            return food.Category.Equals(categoryManager.getEnum(category.Title)) || categoryManager.getEnum(category.Title) == eCategory.None;
        }


        /*private int findMaxPrice()
        {
            int maxPrice = 0;
            foreach (Food food in App.FoodData.lstFood)
            {
                if (maxPrice < food.Price)
                {
                    maxPrice = food.Price;
                }
            }

            return maxPrice;
        }*/

        private string findCategoryTotal()
        {

            int CategoryTotal = 0;

            Category category = (lvCategory.SelectedItem as Category);

            if (category == null)
            {
                return "9";
            }

            foreach (Food food in App.FoodData.lstFood)
            {
                bool isSameCategory = checkCategory(food, category);
                if (isSameCategory)
                {
                    CategoryTotal += food.TotalPrice;
                }

            }
            categoryTotalText.Text = "총합 : " + CategoryTotal.ToString();
            Debug.WriteLine("총합 : " + CategoryTotal.ToString());
            return CategoryTotal.ToString();



        }



        /*public void Button_Click(object sender, RoutedEventArgs e)
        {
            foods.Clear();
            int totalMaxPrice = findMaxPrice();

            foreach (Food food in App.FoodData.lstFood)
            {
                food.Ratio = ((float)food.Price / totalMaxPrice * 350) + 10;

                Debug.WriteLine(food.Ratio);
                foods.Add(food);
            }

            foodChart.ItemsSource = foods;
            foodChart.Items.Refresh();
        }*/

    }
}
