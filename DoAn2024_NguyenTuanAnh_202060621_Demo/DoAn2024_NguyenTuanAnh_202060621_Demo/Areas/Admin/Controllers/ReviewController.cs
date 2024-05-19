using DoAn2024_NguyenTuanAnh_202060621_Demo.Models.AutoAdd;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace DoAn2024_NguyenTuanAnh_202060621_Demo.Areas.Admin.Controllers
{
    public class ReviewController : Controller
    {
        private WenBanHangOnline db = new WenBanHangOnline();
        // GET: Admin/Review
        public ActionResult Index(string Searchtext, int? page)
        {
            if (page == null)
            {
                page = 1;
            }
            var pageNumber = page ?? 1;
            var pageSize = 10;
            IEnumerable<tb_Review> items = db.tb_Reviews.OrderByDescending(x => x.CreatedDate).ToList();
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items= items.Where(x => x.Rate.Contains(Searchtext) || x.tb_Product.Title.Contains(Searchtext) || x.tb_Product.ProductCode.Contains(Searchtext));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
          
            return View(items);
        }
    }
}