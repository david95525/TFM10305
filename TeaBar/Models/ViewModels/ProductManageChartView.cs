using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeaBar.Models.ViewModels
{
    public class ProductManageChartView
    {
        //CategoryQuantity
        public class CategoryQuantity
        {            
            public int CategoryCount { get; set; }
        }
        //Orders
        public string OrderID { get; set; }
        public string UserID { get; set; }
        public string OrderDate { get; set; }
        public int DiscountID { get; set; }

        //OrderDetail
        public int OrderDetailID { get; set; }
        //public string OrderID { get; set; }
        public int ProductID { get; set; }
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
        public int TotalPrice { get; set; }
        public string Note { get; set; }
        public string Customization { get; set; }

        //Products
        //public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int CategoryID { get; set; }
        //public int UnitPrice { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }

        //Categories
        //public int CategoryID { get; set; }
        public string StoreID { get; set; }
        public string CategoryName { get; set; }
        public virtual Stores Stores { get; set; }

        //Stores
        //public string StoreID { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string StorePhone { get; set; }
        public string StoreCity { get; set; }
        public string StoreDistrict { get; set; }

    }
}
