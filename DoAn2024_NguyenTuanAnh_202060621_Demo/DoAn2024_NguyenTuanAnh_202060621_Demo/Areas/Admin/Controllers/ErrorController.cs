using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn2024_NguyenTuanAnh_202060621_Demo.Areas.Admin.Controllers
{
    public class ErrorController : Controller
    {
      
        // GET: Admin/Error
        public ActionResult KhongCoQuyen()
        {
            return View();
        }
    }
}