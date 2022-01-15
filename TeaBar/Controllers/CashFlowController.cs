using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TeaBar.Models;
using TeaBar.Models.CashFlow;
using TeaBar.Models.Utility;


namespace TeaBar.Controllers
{
    public class CashFlowController : Controller
    {
        public IActionResult Test()
        {
            return View();
        }
        #region 付款確認頁面 Paybill
        //決定orderid
        public IActionResult Paybill()
        {
            if (User.Identity.Name == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            string userName = User.Identity.Name;
            //if (HttpContext.Request.Cookies[userName] != null)
            if(HttpContext.Session.Keys.Contains(userName))
            {
                //從cookie讀出
                //string cartstring = HttpContext.Request.Cookies[userName];
                //從session讀出
                string cartstring = HttpContext.Session.GetString(userName);
                List<CartViewModel> carts = JsonConvert.DeserializeObject<List<CartViewModel>>(cartstring);
                //決定orderid
                string orderid = DateTime.Now.ToString("MMddHHmmssyyyy");
                foreach (CartViewModel item in carts)
                {
                    orderid = orderid + item.ProductId.ToString() + item.Quantity.ToString();
                }
                carts[0].OrderID = orderid;
                //cookie寫入
                string jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(carts);
                //CookieOptions option = new CookieOptions();
                //option.Expires = DateTime.Now.AddDays(5);
                //HttpContext.Response.Cookies.Append(userName,jsonstring, option);
                HttpContext.Session.SetString(userName, jsonstring);

                ViewBag.carts = carts;
            }
            return View();
        }
        #endregion
        #region 金流基本資料
        /// <summary>
        /// 金流基本資料
        /// </summary>
        public static string webstring = "https://teabar.azurewebsites.net";
        private CashFlowInfoModel _cashflowInfoModel = new CashFlowInfoModel
         {
            MerchantID = "MS129744283",
            HashKey = "OPvfUPvr2OcBgHGkVcOun9QO2iXxhVjJ",
            HashIV = "ChERSoTztRqSljjP",
            ReturnURL = webstring + "/CashFlow/Cashflowreturn",
            NotifyURL = null,
            CustomerURL = webstring + "/CashFlow/Cashflowcustomer",
            AuthUrl = "https://ccore.spgateway.com/MPG/mpg_gateway",
            CloseUrl = "https://core.newebpay.com/API/CreditCard/Close"
        };
        #endregion
        #region 金流介接訊息傳出 Cashflowpaybillasync 
        /// <summary>
        /// [智付通支付]金流介接
        /// </summary>
        /// <param name="ordernumber">訂單單號</param>
        /// <param name="amount">訂單金額</param>
        /// <param name="payType">請款類型</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult>  Cashflowpaybillasync(string payType,int total,string orderid)
        {
            //目前版本
           string version = "2.0";
            // 目前時間轉換 +08:00, 防止傳入時間或Server時間時區不同造成錯誤
            DateTimeOffset taipeiStandardTimeOffset = DateTimeOffset.Now.ToOffset(new TimeSpan(8, 0, 0));

            TradeInfo tradeInfo = new TradeInfo()
            {
                // * 商店代號
                MerchantID = _cashflowInfoModel.MerchantID,
                // * 回傳格式
                RespondType = "String",
                // * TimeStamp
                TimeStamp = taipeiStandardTimeOffset.ToUnixTimeSeconds().ToString(),
                // * 串接程式版本
                Version = version,
                // * 商店訂單編號
                //MerchantOrderNo = $"T{DateTime.Now.ToString("yyyyMMddHHmm")}",
                MerchantOrderNo = orderid,
                // * 訂單金額
                Amt = total,
                // * 商品資訊
                ItemDesc = "商品資訊(自行修改)",
                // 繳費有效期限(適用於非即時交易)
                ExpireDate = null,
                // 支付完成 返回商店網址
                ReturnURL = _cashflowInfoModel.ReturnURL,
                // 支付通知網址
                NotifyURL = _cashflowInfoModel.NotifyURL,
                // 商店取號網址
                CustomerURL = _cashflowInfoModel.CustomerURL,
                // 支付取消 返回商店網址
                ClientBackURL = webstring,
                // * 付款人電子信箱
                Email = string.Empty,
                // 付款人電子信箱 是否開放修改(1=可修改 0=不可修改)
                EmailModify = 0,
                // 商店備註
                OrderComment = null,
                // 信用卡 一次付清啟用(1=啟用、0或者未有此參數=不啟用)
                CREDIT = null,
                // WEBATM啟用(1=啟用、0或者未有此參數，即代表不開啟)
                WEBATM = null,
                // ATM 轉帳啟用(1=啟用、0或者未有此參數，即代表不開啟)
                TAIWANPAY = null,
                // 設定是否啟用台灣Pay(1=啟用、0或者未有此參數，即代表不開啟)
                ANDROIDPAY=null
                // 設定是否啟用Google Pay(1=啟用、0或者未有此參數，即代表不開啟)
            };
            //信用卡
            if (string.Equals(payType, "CREDIT"))
            {
                tradeInfo.CREDIT = 1;
            }//webatm
            else if (string.Equals(payType, "WEBATM"))
            {
                tradeInfo.WEBATM = 1;
            }
            //ATM轉帳
            else if (string.Equals(payType, "VACC"))
            {
                // 設定繳費截止日期
                tradeInfo.ExpireDate = taipeiStandardTimeOffset.AddDays(1).ToString("yyyyMMdd");
                tradeInfo.VACC = 1;
            }
            //TAIWANPAY
            else if (string.Equals(payType, "TAIWANPAY"))
            {
                // 設定繳費截止日期
                tradeInfo.ExpireDate = taipeiStandardTimeOffset.AddDays(1).ToString("yyyyMMdd");
                tradeInfo.TAIWANPAY= 1;
            }
            //Google Pay
            else if (string.Equals(payType, "ANDROIDPAY"))
            {
                // 設定繳費截止日期
                tradeInfo.ExpireDate = taipeiStandardTimeOffset.AddDays(1).ToString("yyyyMMdd");
                tradeInfo.ANDROIDPAY = 1;
            }

            CashFlowMessage<string> result = new CashFlowMessage<string>()
            {
                IsSuccess = true
            };

            var inputModel = new InputModel
            {
                MerchantID = _cashflowInfoModel.MerchantID,
                Version = version
            };

            // 將model 轉換為List<KeyValuePair<string, string>>, null值不轉
            List<KeyValuePair<string, string>> tradeData = LambdaUtility.ModelToKeyValuePairList<TradeInfo>(tradeInfo);
            // 將List<KeyValuePair<string, string>> 轉換為 key1=Value1&key2=Value2&key3=Value3...
            var tradeQueryPara = string.Join("&", tradeData.Select(x => $"{x.Key}={x.Value}"));
            // AES 加密
            inputModel.TradeInfo = Crypto.EncryptAESHex(tradeQueryPara, _cashflowInfoModel.HashKey, _cashflowInfoModel.HashIV);
            // SHA256 加密
            inputModel.TradeSha = Crypto.EncryptSHA256($"HashKey={_cashflowInfoModel.HashKey}&{inputModel.TradeInfo}&HashIV={_cashflowInfoModel.HashIV}");

            // 將model 轉換為List<KeyValuePair<string, string>>, ewnull值不轉
            List<KeyValuePair<string, string>> postData = LambdaUtility.ModelToKeyValuePairList<InputModel>(inputModel);

            HttpContext.Response.Clear();

            StringBuilder s = new StringBuilder();
            s.Append("<html>");
            s.AppendFormat("<body onload='document.forms[\"form\"].submit()'>");
            s.AppendFormat("<form name='form' action='{0}' method='post'>", _cashflowInfoModel.AuthUrl);
            foreach (KeyValuePair<string, string> item in postData)
            {

                s.AppendFormat("<input type='hidden' name='{0}' value='{1}' />", item.Key, item.Value);
            }

            s.Append("</form></body></html>");

            await HttpContext.Response.WriteAsync(s.ToString());
            await HttpContext.Response.CompleteAsync();
            //Response.Write(s.ToString());
            //Response.End();
            return Content(string.Empty);
        }
        #endregion
        //測試卡號：4000-2211-1111-1111
        // 有效期限：輸入比今天大即可
        //末三碼：任意填寫
        #region 信用卡返回商店
        /// <summary>
        /// [智付通]金流介接(結果: 支付完成 返回商店網址)
        /// </summary>
        [HttpPost]
        public ActionResult Cashflowreturn()
        {

            HttpContext.Request.LogFormData("Cashflowreturn(支付完成)");

            // Status 回傳狀態 
            // MerchantID 回傳訊息
            // TradeInfo 交易資料AES 加密
            // TradeSha 交易資料SHA256 加密
            // Version 串接程式版本
            NameValueCollection collection = new NameValueCollection();
            foreach (var item in HttpContext.Request.Form)
            {
                string key = item.Key;
                string value= item.Value.ToString();
                collection.Add(key, value);
            }
            if (collection["MerchantID"] != null && string.Equals(collection["MerchantID"], _cashflowInfoModel.MerchantID) &&
                collection["TradeInfo"] != null && string.Equals(collection["TradeSha"], Crypto.EncryptSHA256($"HashKey={_cashflowInfoModel.HashKey}&{collection["TradeInfo"]}&HashIV={_cashflowInfoModel.HashIV}")))
            {
                var decryptTradeInfo = Crypto.DecryptAESHex(collection["TradeInfo"], _cashflowInfoModel.HashKey, _cashflowInfoModel.HashIV);

                // 取得回傳參數(ex:key1=value1&key2=value2),儲存為NameValueCollection
                NameValueCollection decryptTradeCollection = HttpUtility.ParseQueryString(decryptTradeInfo);
                OutputDataModel convertModel = LambdaUtility.DictionaryToObject<OutputDataModel>
                    (decryptTradeCollection.AllKeys.ToDictionary(k => k, k => decryptTradeCollection[k]));
       
                ViewBag.result = convertModel;
                return View();
            }
            else
            {
                Console.WriteLine("MerchantID/TradeSha驗證錯誤");
            }

            return Content(string.Empty);
        }
        #endregion
        #region ATM返回商店
        ///Atm轉帳
        [HttpPost]
        public ActionResult Cashflowcustomer()

        {
            Request.LogFormData("Cashflowcustomer(資料回傳)");

            // Status 回傳狀態 
            // MerchantID 回傳訊息
            // TradeInfo 交易資料AES 加密
            // TradeSha 交易資料SHA256 加密
            // Version 串接程式版本
            NameValueCollection collection = new NameValueCollection();
            foreach (var item in HttpContext.Request.Form)
            {
                string key = item.Key;
                string value = item.Value.ToString();
                collection.Add(key, value);
            }
            if (collection["MerchantID"] != null && string.Equals(collection["MerchantID"], _cashflowInfoModel.MerchantID) &&
                collection["TradeInfo"] != null && string.Equals(collection["TradeSha"], Crypto.EncryptSHA256($"HashKey={_cashflowInfoModel.HashKey}&{collection["TradeInfo"]}&HashIV={_cashflowInfoModel.HashIV}")))
            {
                var decryptTradeInfo = Crypto.DecryptAESHex(collection["TradeInfo"], _cashflowInfoModel.HashKey, _cashflowInfoModel.HashIV);

                // 取得回傳參數(ex:key1=value1&key2=value2),儲存為NameValueCollection
                NameValueCollection decryptTradeCollection = HttpUtility.ParseQueryString(decryptTradeInfo);
                OutputDataModel convertModel = LambdaUtility.DictionaryToObject<OutputDataModel>
                    (decryptTradeCollection.AllKeys.ToDictionary(k => k, k => decryptTradeCollection[k]));
                ViewBag.result = convertModel;
                return View("Cashflowreturn");
            }
            else
            {
                Console.WriteLine("MerchantID/TradeSha驗證錯誤");
            }

            return Content(string.Empty);
        }
        #endregion
        /// <summary>
        /// 執行HttpClient Post
        /// </summary>
        /// <param name="url">Post網址</param>
        /// <param name="formContent">form參數</param>
        /// <returns></returns>
        private async Task<string> ExePostForm(string url, FormUrlEncodedContent formContent)
        {
            string responseBody = string.Empty;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.PostAsync(url, formContent);

                    if (response.IsSuccessStatusCode)
                    {
                        responseBody = await response.Content.ReadAsStringAsync();
                        // 紀錄回傳資訊
                        LogUtility.WriteLog(responseBody);
                    }
                }
            }
            catch
            {
                
            }

            return responseBody;
        } // ExePostForm()


    

    }
}
