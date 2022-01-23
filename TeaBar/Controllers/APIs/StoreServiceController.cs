using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TeaBar.Data;
using TeaBar.Models;
using TeaBar.Models.ViewModels;

namespace TeaBar.Controllers.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreServiceController : ControllerBase
    {
        //注入db
        private readonly ApplicationDbContext _dbcontext;
        //注入httpclinet
        private readonly IHttpClientFactory _factory;

        public StoreServiceController(ApplicationDbContext applicationDbContext, IHttpClientFactory httpClientFactory)
        {
            _dbcontext = applicationDbContext;
            _factory = httpClientFactory;
        }



        [HttpGet]
        [Route("city")]
        [Produces("application/json")]
        public List<string> GetCityList() //取得城市清單
        {
            var result = _dbcontext.Stores
                       .OrderBy(c => c.StoreName)
                       .Select(c => c.StoreCity).Distinct().ToList();
            return result;
        }

        [HttpGet]
        [Route("dist")]
        [Produces("application/json")]
        public List<string> GetDistList([FromQuery] string city) //取得區域清單
        {
            var result = _dbcontext.Stores
                       .Where(d => d.StoreCity == city)
                       .Select(c => c.StoreDistrict).Distinct().ToList();
            return result;
        }

        [HttpGet]
        [Route("store/{dist}")]
        [Produces("application/json")]
        public List<Stores> GetStoreList([FromRoute(Name = "dist")] string dist) //取得分店資料
        {
            var result = _dbcontext.Stores
                       .Where(d => d.StoreDistrict == dist)
                       .Select(c => c);
            List<Stores> data = result.ToList();
            return data;
        }

        [HttpGet]
        [Route("store")]
        [Produces("application/json")]
        public async Task<String> GetStoreData() //取得所有分店及經緯度資料
        {
            //建立要傳回前端的viewmodel集合
            List<StoreViewModel> vmList = new List<StoreViewModel>();
            //取出分店所有資料
            List<Stores> StoreData = _dbcontext.Stores
                .Select(s => s).ToList();


            foreach (var item in StoreData)
            {
                //取出分店地址清單
                var StoreAddress = $"{item.StoreCity}{item.StoreDistrict}{item.StoreAddress}";
                //.Select(s => $"{s.StoreCity}{s.StoreDistrict}{s.StoreAddress}").ToList();
                //建立viewmodel物件
                StoreViewModel vm = new StoreViewModel() { };
                //將資料存進vm
                vm.StoreID = item.StoreID;
                vm.StoreName = item.StoreName;
                vm.StorePhone = item.StorePhone;
                vm.StoreCity = item.StoreCity;
                vm.StoreDistrict = item.StoreDistrict;
                vm.StorePicture = "/images/map/store";
                vm.StoreAddress = StoreAddress;

                //傳到google geocoding api取得經緯度位置
                String url = $"https://maps.googleapis.com/maps/api/geocode/json?address={StoreAddress}&language=zh-TW&key=AIzaSyBayPidrH0yQql4BzYCXr7SIs08AEEcMgU";
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                HttpClient client = _factory.CreateClient();
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    Geomery geoData = new Geomery(); //new Geomery物件
                    var result = await response.Content.ReadAsStringAsync();
                    //把回傳的json資料反序列化為物件
                    geoData = Newtonsoft.Json.JsonConvert.DeserializeObject<Geomery>(result);
                    //Geomery物件裡的經緯度存進vm
                    vm.Lat = geoData.results[0].geometry.location.lat;
                    vm.Lng = geoData.results[0].geometry.location.lng;
                }
                else
                {
                    return string.Empty;
                }
                //把vm物件存到list
                vmList.Add(vm);

            }
            //vmList序列化為json
            String jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(vmList);
            //vmList傳到前端
            return jsonData;
        }
    }
}
