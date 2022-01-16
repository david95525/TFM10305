using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeaBar.Models
{
    public class Products
    {
        [Key]
        public int ProductID { get; set; }
        public string ProductName { get; set; }

        [ForeignKey("Categories")]
        public int CategoryID { get; set; }
        public int UnitPrice { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }

        public virtual Categories Categories { get; set; }
    }
}