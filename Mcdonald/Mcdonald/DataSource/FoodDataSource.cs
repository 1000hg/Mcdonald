using LbMcdonald.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Mcdonald.DataSource
{
    public class FoodDataSource
    {
        bool isLoaded = false;

        public List<Food> lstFood;

        public void Load()
        {
            if (isLoaded) return;

            lstFood = new List<Food>()
            {
                new Food() { Name="Big Mac", Price = 5000, Count = 0, Category = eCategory.Burger, ImagePath = "/Assets/FoodImage/big mac.jpg"},
                new Food() { Name="Chcolate Shake", Price = 5000, Count = 0, Category = eCategory.Side, ImagePath = "/Assets/FoodImage/chcolate shake.jpg"},
                new Food() { Name="CheeseBurger", Price = 5000, Count = 0, Category = eCategory.Burger, ImagePath = "/Assets/FoodImage/cheeseburger.jpg"},
                new Food() { Name="Coca Cola", Price = 5000, Count = 0, Category = eCategory.Drink, ImagePath = "/Assets/FoodImage/coca cola.jpg"},
                new Food() { Name="Fanta", Price = 5000, Count = 0, Category = eCategory.Drink, ImagePath = "/Assets/FoodImage/fanta.jpg"},
                new Food() { Name="Salad", Price = 5000, Count = 0, Category = eCategory.Side, ImagePath = "/Assets/FoodImage/salad.jpg"},
                new Food() { Name="Sprite", Price = 5000, Count = 0, Category = eCategory.Drink, ImagePath = "/Assets/FoodImage/sprite.jpg"},
                new Food() { Name="Vanilla Cone", Price = 5000, Count = 0, Category = eCategory.Side, ImagePath = "/Assets/FoodImage/vanilla cone.jpg"},
                new Food() { Name="Egg Mcmuffin", Price = 5000, Count = 0, Category = eCategory.Breakfast, ImagePath = "/Assets/FoodImage/egg mcmuffin.jpg"},
                new Food() { Name="Strawberry Shake", Price = 5000, Count = 0, Category = eCategory.Side, ImagePath = "/Assets/FoodImage/strawberry shake.jpg"},
                new Food() { Name="World Famous Fries", Price = 5000, Count = 0, Category = eCategory.Side, ImagePath = "/Assets/FoodImage/world famous fries.jpg"},
                new Food() { Name="Sausage Biscuit", Price = 5000, Count = 0, Category = eCategory.Breakfast, ImagePath = "/Assets/FoodImage/sausage biscuit.jpg"},
                new Food() { Name="Sausage Biscuit With Egg", Price = 5000, Count = 0, Category = eCategory.Breakfast, ImagePath = "/Assets/FoodImage/sausage biscuit with egg.jpg"}
            };

            isLoaded = true;
        }
    }
}
