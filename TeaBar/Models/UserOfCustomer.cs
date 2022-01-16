using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeaBar.Models
{
    public class UserOfCustomer
    {
        [Key, ForeignKey("ApplicationUsers")]
        public string UserID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime Birth { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Address { get; set; }

        public virtual ApplicationUsers ApplicationUsers { get; set; }
    }
}