using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mcdonald.Model
{
    class Statistics
    {
        public DateTime date { get; set; }
        public List<Menu> menuList { get; set; }

        public Statistics(DateTime date, List<Menu> menuList)
        {
            this.date = date;
            this.menuList = menuList;
        }
    }
}
