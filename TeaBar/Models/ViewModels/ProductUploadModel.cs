using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace TeaBar.Controllers
{
    public class ProductUploadModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int CategoryID { get; set; }
        public int UnitPrice { get; set; }
        public string Description { get; set; }
        public List<IFormFile> Picture { get; set; }
    }
}