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

namespace Mcdonald
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private CategoryManager categoryManager = new CategoryManager();

        private List<Food> foods = new List<Food>();

        private List<Food> selectedFoods = new List<Food>();
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = App.FoodData;
            Debug.WriteLine("Maindow_load");
            App.CategoryData.Load();
            App.FoodData.Load();
            lvCategory.ItemsSource = App.CategoryData.lstCategory;
            lvFood.ItemsSource = App.FoodData.lstFood;
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

        private void BtnMinus_Click(object sender, RoutedEventArgs e)
        {
            Button button = (sender as Button);
            Food food = button.DataContext as Food;
            if (food == null) return;
            minusFood(food);
            lvFood.Items.Refresh();
        }

        private void plusFood(Food food)
        {
            food.Count++;
            if (food.Count == 1)
            {
                selectedFoods.Add(food);
            }
            updateSelectedFood();
        }

        private void minusFood(Food food)
        {
            food.Count--;
            if (food.Count == 0)
            {
                selectedFoods.Remove(food);
            }
            updateSelectedFood();
        }

        private void updateSelectedFood()
        {
            lvSelected.ItemsSource = selectedFoods;
            lvSelected.Items.Refresh();
        }
    }
}
