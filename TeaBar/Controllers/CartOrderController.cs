﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TeaBar.Data;
using TeaBar.Models;


namespace TeaBar.Controllers
{
    public class CartOrderController : Controller
    {
        private readonly ApplicationDbContext _db;
        //注入進來的logger物件
      

        //建構子注入
        public CartOrderController(ApplicationDbContext context)
        {
            _db = context; 
        }
        public IActionResult ShoppingCart()
        {
            if (User.Identity.Name == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            
            if (HttpContext.Session.Keys.Contains("temp"))
            {
                string jsonstring = HttpContext.Session.GetString("temp");
                List<CartViewModel> temp
                    = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CartViewModel>>(jsonstring);
                HttpContext.Session.Remove("temp");
                string userName = User.Identity.Name;
                foreach (CartViewModel item in temp)
                {
                    item.UserID = userName;
                }
                HttpContext.Session.SetString("username", userName);
                string jsonstringafter = Newtonsoft.Json.JsonConvert.SerializeObject(temp);
                HttpContext.Session.SetString(userName, jsonstringafter);
            } 
            
            return View();
        }
 

        public IActionResult OrderSteps()
        {
            if (User.Identity.Name == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            return View();
        }

    }
}
