using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TeaBar.Data;
using TeaBar.Models.ViewModels;

namespace TeaBar.Controllers.APIs
{
    public class StoreInfoServiceController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public StoreInfoServiceController(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        //門市資料查詢
        [HttpGet]
        public string storeInfoQry()    //門市資料查詢
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = _dbContext.ApplicationUsers.Where(u => u.Id == id).FirstOrDefault();
            var resultS = _dbContext.Stores.Where(s => s.StoreID == id).FirstOrDefault();

            var storeInfo = new StoreInfoViewModel
            {
                Email = result.Email,
                StoreName = resultS.StoreName,
                StorePhone = resultS.StorePhone,
                StoreCity = resultS.StoreCity,
                StoreDistrict = resultS.StoreDistrict,
                StoreAddress = resultS.StoreAddress
            };
            var data = Newtonsoft.Json.JsonConvert.SerializeObject(storeInfo);

            return data;
        }

        //門市資訊更新
        [HttpPut]
        [Consumes("application/json")]
        public string storeInfoUpdate([FromBody]StoreInfoViewModel store) //門市資訊更新
        {
            string msg = null;

            var qryResult = _dbContext.ApplicationUsers.Where(u => u.Email == store.Email).FirstOrDefault();
            var qryStore = _dbContext.Stores.Where(s => s.StoreID == qryResult.Id).FirstOrDefault();

            if (qryResult == null)
            {
                msg = $"此帳號：{store.Email}不存在!";
            }
            else
            {
                _dbContext.Entry(qryResult).CurrentValues.SetValues(store);
                _dbContext.Entry(qryStore).CurrentValues.SetValues(store);

                try
                {
                    _dbContext.SaveChanges();
                    msg = "資料更新成功！";
                }
                catch(DbUpdateException ex)
                {
                    msg = "資料更新失敗...";
                }
            }
            return msg;
        }
    }
}
