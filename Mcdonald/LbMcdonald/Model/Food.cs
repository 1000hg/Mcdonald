using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LbMcdonald.Model
{
    public class Food
    {
        public Food()
        {

        }
        
        public Food(Food food)
        {
            this.Name = food.Name;
            this.Price = food.Price;
            this.Count = food.Count;
            this.ImagePath = food.ImagePath;
            this.Category = food.Category;
            this.TotalPrice = food.TotalPrice;
        }

        public String Name { get; set; }
        public int Price { get; set; }

        private int count;
        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                count = value;

                if (count < 0)
                {
                    count = 0;
                }
            }
        }
        public String ImagePath { get; set; }
        public eCategory Category { get; set; }

        public int TotalPrice
        {
            get
            {
                return Count * Price;
            }
            set
            {
                return;
            }
        }

        public int StatisticTotal { get; set; }

        public float Ratio { get; set; }

        public DateTime Date { get; set; }
    }
}
