using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeaBar.Models.OrderViewModels
{
    public class Orderview
    {



        public int OrderDetailID { get; set; }
        public string OrderID { get; set; }
        public int ProductID { get; set; }
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
        public int TotalPrice { get; set; }
        public string Note { get; set; }
        public string Customization { get; set; }

        public string UserID { get; set; }
        public string OrderDate { get; set; }
        public int DiscountID { get; set; }

        public string ProductName { get; set; }
        public int CategoryID { get; set; }

        public string Description { get; set; }
        public string Picture { get; set; }
    }
}
