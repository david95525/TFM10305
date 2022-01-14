using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeaBar.Models.ViewModels
{
    public class RegisterOfMemberViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime Birth { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
    }
}
