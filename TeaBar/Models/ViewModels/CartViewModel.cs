using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeaBar.Models
{
    public class CartViewModel
    {
        public string OrderID { get; set; }
        public string UserID { get; set; }
        public string StoreID { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
        public int DiscountId { get; set; }
        public double Discount { get; set; }
        public int Subtotal { get; set; }
        public string Sweetness { get; set; }
        public string Ice { get; set; }
       public List<string> Ingredient { get; set; }
        public string Size { get; set; }
        public string Picture { get; set; }
        public string Note { get; set; }
       
        public string Customization { get; set; }
    }
}
