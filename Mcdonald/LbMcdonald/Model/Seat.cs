﻿using LbMcdonald;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LbMcdonald.Model
{ 
    public class Seat
    {

        public Seat()
        {

        }

        public Seat(Seat seat)
        {
            this.Idx = seat.Idx;
            this.Payment = seat.Payment;
            seat.FoodList.ForEach((food) =>
            {
                Food tempFood = new Food(food);
                this.FoodList.Add(tempFood);
            });
            this.TotalPrice = seat.TotalPrice;
        }

        public int Idx { get; set; }

        private List<Food> foodList = new List<Food>();
        public List<Food> FoodList
        {
            get
            {
                return foodList;
            }
            set
            {
                foodList = value;
            }
        }
        public ePayment Payment { get; set; }
        public int TotalPrice
        {
            get
            {
                int totalPrice = 0;

                foreach (Food food in FoodList)
                {
                    totalPrice += food.TotalPrice;
                }

                return totalPrice;
            }
            set
            {
                return;
            }
        }
    }
}
