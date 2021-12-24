using System;
using System.Collections.Generic;

#nullable disable

namespace TFM10305.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public string CustomerId { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime? Birth { get; set; }
        public string Gender { get; set; }
        public DateTime CreateDate { get; set; }
        public string City { get; set; }
        public string District { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
