using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeaBar.Models.CashFlow
{
    public class CashFlowMessage
    {
        /// <summary>是否成功</summary>
        public bool IsSuccess { get; set; }
        /// <summary>訊息</summary>
        public string Message { get; set; }
    }
}
