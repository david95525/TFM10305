using System;
using System.Collections.Generic;

#nullable disable

namespace TFM10305.Models
{
    public partial class Cart
    {
        public string CustomerId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int? UnitPrice { get; set; }
        public int? Quantity { get; set; }
        public string DiscountId { get; set; }
        public int? Discount { get; set; }
        public string Sugarlevel { get; set; }
        public string Icelevel { get; set; }
        public string Ingredient { get; set; }
        public string Description { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Discount DiscountNavigation { get; set; }
        public virtual Product Product { get; set; }
    }
}
