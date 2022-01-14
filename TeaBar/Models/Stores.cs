using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeaBar.Models
{
    public class Stores
    {
        [Key, ForeignKey("ApplicationUsers")]
        public string StoreID { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string StorePhone { get; set; }
        public string StoreCity { get; set; }
        public string StoreDistrict { get; set; }

        public virtual ApplicationUsers ApplicationUsers { get; set; }
        public virtual IList<Categories> Categories { get; set; }
    }
}