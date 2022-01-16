using System.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Protocols;

namespace TeaBar.Models.Utility
{
    public class LogUtility
    {

        private readonly IWebHostEnvironment _WebHostEnvironment;

  
        public LogUtility(IWebHostEnvironment WebHostEnvironment)
        {
            _WebHostEnvironment = WebHostEnvironment;
        }
        /// <summary>
        /// 寫入Log檔
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteLog(string msg)
        {
            //try
            //{
            //    string logDirectory = MapPath(GetAppSetting("LogDirectory"));

            //    if (!Directory.Exists(logDirectory))
            //    {
            //        Directory.CreateDirectory(logDirectory);
            //    }

            //    string nowString = DateTime.Now.ToString("yyyyMMdd");
            //    string logFile = logDirectory + string.Format("log_{0}.txt", nowString);

            //    File.AppendAllText(logFile, string.Format("{0}: {1}{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), msg, Environment.NewLine));
            //}
            //catch (Exception ex)
            //{
            //}
        }
        /// <summary>
        /// 取得Web.config的AppSettings設定
        /// </summary>
        /// <param name="appSettingKey">讀取的appSettingKey</param>
        /// <returns></returns>
        //public static string GetAppSetting(string appSettingKey)
        //{
        //    string result = string.Empty;

        //    if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[appSettingKey]))
        //    {
        //        result = ConfigurationManager.AppSettings[appSettingKey].Trim();
        //    }

        //    return result;

        //}

        /// <summary>
        /// 取得檔案實體路徑
        /// </summary>
        /// <param name="path">檔案路徑</param>
        /// <returns></returns>

    
        public string MapPath(string path)
        {
            string webRootPath = _WebHostEnvironment.WebRootPath;
            string contentRootPath = _WebHostEnvironment.ContentRootPath;
            if (webRootPath != null)
            {

                return webRootPath + "\n" + contentRootPath;
            }

            return webRootPath + "\n" + contentRootPath;
        }
    }
}
