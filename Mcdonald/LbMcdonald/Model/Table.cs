using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mcdonald.Model
{
    enum PaymentType { CARD, CASH }
    class Table
    {
        public int idx { get; set; }
        public List<Menu> menuList { get; set; }
        public PaymentType payment { get; set; }
        public int totalPrice { get; set; }

        public Table(int idx, List<Menu> menuList, PaymentType payment, int totalPrice)
        {
            this.idx = idx;
            this.menuList = menuList;
            this.payment = payment;
            this.totalPrice = totalPrice;
        }
    }
}
