using Microsoft.AspNetCore.Http;
using System.Text;

namespace TeaBar.Models.Utility
{
    public static class RazorExtension
    {
        public static void LogFormData(this HttpRequest request, string funcName)
        {
            var formData = request.Form;
            //LogUtil.WriteLog(JsonConvert.SerializeObject(formData));

            var builder = new StringBuilder();
            builder.AppendLine(funcName);
            //foreach (string key in formData)
            //{
            //    var val = formData[key];
            //    builder.AppendLine(string.Format("{0} : {1}", key, val));
            //}
            //LogUtil.WriteLog(builder.ToString());
        }
    }
}
