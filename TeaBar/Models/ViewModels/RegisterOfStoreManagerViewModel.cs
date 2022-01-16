using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeaBar.Models.ViewModels
{
    public class RegisterOfStoreManagerViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string StorePhone { get; set; }
        public string StoreCity { get; set; }
        public string StoreDistrict { get; set; }
    }
}
