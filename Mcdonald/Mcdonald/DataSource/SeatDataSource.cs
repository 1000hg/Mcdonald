using LbMcdonald.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mcdonald.DataSource
{
    public class SeatDataSource
    {
        bool isLoaded = false;

        public List<Seat> lstSeat;

        public void Load()
        {
            if (isLoaded) return;

            if (lstSeat == null)
            {
                lstSeat = new List<Seat>()
                {
                    new Seat() { Idx = 1 },
                    new Seat() { Idx = 2 },
                    new Seat() { Idx = 3 },
                    new Seat() { Idx = 4 },
                    new Seat() { Idx = 5 },
                    new Seat() { Idx = 6 },
                    new Seat() { Idx = 7 },
                    new Seat() { Idx = 8 },
                };
            }

            isLoaded = true;
        }
    }
}
