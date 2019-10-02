using Mcdonald.DataSource;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Mcdonald
{
    public partial class App : Application
    {
        public static CategoryDataSource CategoryData = new CategoryDataSource();
        public static FoodDataSource FoodData = new FoodDataSource();
        public static SeatDataSource SeatData = new SeatDataSource();
    }
}
