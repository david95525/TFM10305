using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeaBar.Models.OrderViewModels
{
    public class TopOrederview
    {

        //Orders
        public string OrderID { get; set; }

        public string UserID { get; set; }
        public string OrderDate { get; set; }
        public int DiscountID { get; set; }

        //UserOfCustomer
        
        
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime Birth { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Address { get; set; }

        //Discount
        
        public double DiscountRule { get; set; }
        public string Description { get; set; }

        //Stores
        public string StoreID { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string StorePhone { get; set; }
        public string StoreCity { get; set; }
        public string StoreDistrict { get; set; }
    }
}
