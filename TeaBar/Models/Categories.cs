using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeaBar.Models
{
    public class Categories
    {
        [Key]
        public int CategoryID { get; set; }


    
        public string CategoryName { get; set; }

        public virtual Stores Stores { get; set; }
        public virtual IList<Products> Products { get; set; }
    }
}