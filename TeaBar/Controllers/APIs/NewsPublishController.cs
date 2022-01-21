using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeaBar.Data;
using TeaBar.Models;

namespace TeaBar.Controllers.APIs
{
    public class NewsPublishController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public NewsPublishController(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public string SaveNews([FromBody]News data)     //儲存公告發佈內容
        {
            string msg = null;        

            var result = new News
            {
                Topic = data.Topic,
                ContentText = data.ContentText,
                Time = DateTime.Now
            };

            if (result != null)
            {
                try
                {
                    _dbContext.Add(result);
                    _dbContext.SaveChanges();
                    msg = "資料儲存成功！";
                }
                catch (DbUpdateException ex)
                {
                    msg = "資料儲存失敗...";
                }
            }
            return msg;
        }


        [HttpGet]
        public List<News> GetNews()     //取出公告
        {
            List<News> data = _dbContext.News.Select(n=>n).OrderByDescending(o=>o.Time).ToList<News>();
            return data;
        }
    }
}
