using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeaBar.Models.ViewModels
{
    public class HistoryOrderDetailsViewModel
    {
        public string ProductName { get; set; }
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }
        public string Customization { get; set; }
        public int TotalPrice { get; set; }
    }
}
