using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeaBar.Models
{
    public class OrderDetails
    {
        [Key]
        public int OrderDetailID { get; set; }

        [ForeignKey("Orders")]
        public string OrderID { get; set; }

        [ForeignKey("Products")]
        public int ProductID { get; set; }

        public int UnitPrice { get; set; }

        public int Quantity { get; set; }
        public int TotalPrice { get; set; }

        public string Note { get; set; }
        public string Customization { get; set; }

        public virtual Orders Orders { get; set; }
        public virtual Products Products { get; set; }
    }
}