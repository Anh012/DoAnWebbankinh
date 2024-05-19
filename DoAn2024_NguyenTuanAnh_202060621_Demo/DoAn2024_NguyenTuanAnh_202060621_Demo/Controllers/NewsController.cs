using DoAn2024_NguyenTuanAnh_202060621_Demo.Models.AutoAdd;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn2024_NguyenTuanAnh_202060621_Demo.Controllers
{
    public class NewsController : Controller
    {
        private WebBanHangOnline db = new WebBanHangOnline();
        // GET: News
        public ActionResult Index(int? page)
        {
            var items = db.tb_News.Where(n => n.IsActive).OrderByDescending(x => x.CreatedDate).ToList();
            if (page == null)
            {
                page = 1;
            }
            var pageNumber = page ?? 1;
            var pageSize = 6;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Partial_News_Home()
        {
            var items = db.tb_News.ToList();
            return PartialView(items);
        }
        public ActionResult NewNews()
        {
            var items = db.tb_News.Where(n => n.IsActive)
                          .OrderByDescending(n => n.CreatedDate)
                          .Take(5)
                          .ToList();

            return PartialView("NewNews", items);
        }
        public ActionResult Detail(int id)
        {
            var item = db.tb_News.Find(id);
            return View(item);
        }
    }
}