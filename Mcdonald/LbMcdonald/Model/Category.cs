using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LbMcdonald.Model
{
    public enum eCategory
    {
        None = 0,
        Breakfast,
        Burger,
        Drink,
        Side
    }

    public class Category
    {
        public String Title { get; set; }
        public String ImagePath { get; set; }
    }
}
