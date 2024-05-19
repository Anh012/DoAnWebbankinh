using DoAn2024_NguyenTuanAnh_202060621_Demo.Models.AutoAdd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn2024_NguyenTuanAnh_202060621_Demo.Controllers
{
    public class AccessController : Controller
    {
        WebBanHangOnline db = new WebBanHangOnline();
        [HttpGet]
        // GET: Access
        public ActionResult Login()
        {
            
            return View();
        }
    }
}