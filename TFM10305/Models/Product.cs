using System;
using System.Collections.Generic;

#nullable disable

namespace TFM10305.Models
{
    public partial class Product
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string CategoryId { get; set; }
        public int UnitPrice { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }
    }
}
