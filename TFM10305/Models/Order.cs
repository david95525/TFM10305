using System;
using System.Collections.Generic;

#nullable disable

namespace TFM10305.Models
{
    public partial class Order
    {
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
        public string StoreId { get; set; }
        public DateTime OrderDate { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
