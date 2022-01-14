using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TeaBar.Models.CountyDistrictModel;

namespace TeaBar.Controllers
{
    public class CountyDistrictController : Controller
    {
        [HttpGet]
        public IActionResult readCounty() //讀取縣市資料
        {
            StreamReader r = new StreamReader("./wwwroot/countydistrict/county.json");
            string jsonString = r.ReadToEnd();
            List<CountyData> countyresult = JsonConvert.DeserializeObject<List<CountyData>>(jsonString);
            return Json(countyresult);
        }

        [HttpPost]
        public IActionResult readDistrict([FromBody] string countyname) //讀取鄉鎮市區資料
        {
            var countycode = "";
            StreamReader rc = new StreamReader("./wwwroot/countydistrict/county.json");
            string jsonStringCounty = rc.ReadToEnd();
            List<CountyData> countyresult = JsonConvert.DeserializeObject<List<CountyData>>(jsonStringCounty);
            foreach (CountyData item in countyresult)
            {
                if (item.Countyname == countyname)
                {
                    countycode = item.Countycode;
                }
            }

            StreamReader rd = new StreamReader($"./wwwroot/countydistrict/town{countycode}.json");
            string jsonString = rd.ReadToEnd();
            List<DistrictData> districtresult = JsonConvert.DeserializeObject<List<DistrictData>>(jsonString);
            return Json(districtresult);
        }
    }
}
