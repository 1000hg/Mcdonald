using LbMcdonald;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LbMcdonald.Model
{ 
    class Table
    {
        public int Idx { get; set; }
        public List<Food> FoodList { get; set; }
        public ePayment Payment { get; set; }
        public int TotalPrice { get; set; }
    }
}
