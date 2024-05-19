using DoAn2024_NguyenTuanAnh_202060621_Demo.Models.AutoAdd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn2024_NguyenTuanAnh_202060621_Demo.Controllers
{
    public class MenuController : Controller
    {
        private WebBanHangOnline db = new WebBanHangOnline();
        // GET: Menu
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MenuTop()
        {
            var items = db.tb_Category.Where(x => x.IsActive).OrderBy(x => x.Position).ToList();
            return PartialView("_MenuTop", items);
        }
        public ActionResult MenuProductCategory()
        {
            var items = db.tb_ProductCategory.ToList();
            return PartialView("_MenuProductCategory", items);
        }
        public ActionResult MenuBot()
        {
            var items = db.tb_ProductCategory.ToList();
            return PartialView("_MenuBot", items);
        }
        public ActionResult MenuLeft(int? id)
        {
            if (id != null)
            {
                ViewBag.CateId = id;
            }
            var items = db.tb_ProductCategory.ToList();
            return PartialView("_MenuLeft", items);
        }
    }
}