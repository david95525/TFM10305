using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeaBar.Models
{
    public class Orders
    {
        [Key]
        public string OrderID { get; set; }

        [ForeignKey("ApplicationUsers")]
        public string UserID { get; set; }
        public string OrderDate { get; set; }

        [ForeignKey("Discount")]
        public int DiscountID { get; set; }

        public virtual ApplicationUsers ApplicationUsers { get; set; }

        public virtual Discount Discount { get; set; }

        public virtual IList<OrderDetails> OrderDetails { get; set; }
    }
}