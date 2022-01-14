using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeaBar.Models
{
    public class ApplicationUsers : IdentityUser
    {       
        public virtual UserOfCustomer UserOfCustomer { get; set; }
        public virtual Stores Stores { get; set; }
        public virtual IList<Orders> Orders { get; set; }
    }
}
