using LbMcdonald.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mcdonald.Manager
{
    class CategoryManager
    {
        public String getString(eCategory e)
        {
            switch (e)
            {
                case eCategory.Breakfast:
                    return "Breakfast";
                case eCategory.Burger:
                    return "Burgers";
                case eCategory.Drink:
                    return "Drinks";
                case eCategory.Side:
                    return "Side & Snack";
                default:
                    return "All Menu";
            }
        }

        public eCategory getEnum(String s)
        {
            switch (s)
            {
                case "Breakfast":
                    return eCategory.Breakfast;
                case "Burgers":
                    return eCategory.Burger;
                case "Drinks":
                    return eCategory.Drink;
                case "Side & Snack":
                    return eCategory.Side;
                default:
                    return eCategory.None;
            }
        }
    }
}
