using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeaBar.Models.ViewModels
{
    public class OrderProductsViewModel
    {
        public int productID { get; set; }
        public int subtotal { get; set; }
        public int quentity { get; set; }
        public size size { get; set; }
        public string sweetness { get; set; }
        public string ice { get; set; }
        public ingredient[] ingredient { get; set; }
    }

    public class size
    {
        public string name { get; set; }
        public int price { get; set; }
    }

    public class ingredient
    {
        public int price { get; set; }
        public string name { get; set; }
    }
}
