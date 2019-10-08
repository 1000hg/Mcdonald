using LbMcdonald.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mcdonald.DataSource
{
    public class CategoryDataSource
    {
        bool isLoaded = false;

        public List<Category> lstCategory;

        public void Load()
        {
            if (isLoaded) return;

            if (lstCategory == null)
            {
                lstCategory = new List<Category>()
                {
                    new Category() { Title = "All Menu", ImagePath = "/Assets/CategoryImage/all.jpg"},
                    new Category() { Title = "Breakfast", ImagePath = "/Assets/CategoryImage/breakfast.jpg"},
                    new Category() { Title = "Burgers", ImagePath = "/Assets/CategoryImage/burger.jpg"},
                    new Category() { Title = "Drinks", ImagePath = "/Assets/CategoryImage/drink.jpg"},
                    new Category() { Title = "Side & Snack", ImagePath = "/Assets/CategoryImage/side.jpg"}
                };
            }

            isLoaded = true;
        }
    }
}
