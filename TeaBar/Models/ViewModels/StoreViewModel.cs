using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeaBar.Models.ViewModels
{
    public class StoreViewModel
    {
        //分店資訊
        public String StoreID { get; set; }
        public String StoreName { get; set; }
        public String StoreAddress { get; set; }
        public String StoreCity { get; set; }
        public String StoreDistrict { get; set; }
        public String StorePhone { get; set; }
        public String StorePicture { get; set; }
        //門市地址經緯度
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
