using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeaBar.Models
{
    [Table("Discounts")]
    public class Discount
    {
        [Key]
        public int DiscountID { get; set; }
        public double DiscountRule { get; set; }
        public string Description { get; set; }

        public List<Orders> Orders { get; set; }
    }
}