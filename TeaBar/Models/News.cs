using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TeaBar.Models
{
    public class News
    {
        [Key]
        public int NewsID { get; set; }
        public string Topic { get; set; }
        public string ContentText { get; set; }
        public DateTime Time { get; set; }
    }
}
