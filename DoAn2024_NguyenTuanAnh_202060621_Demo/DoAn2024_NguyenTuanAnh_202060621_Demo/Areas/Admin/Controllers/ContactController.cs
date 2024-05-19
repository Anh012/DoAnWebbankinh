using DoAn2024_NguyenTuanAnh_202060621_Demo.Models.AutoAdd;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn2024_NguyenTuanAnh_202060621_Demo.Areas.Admin.Controllers
{
    public class ContactController : Controller
    {
        private WenBanHangOnline db = new WenBanHangOnline();
        // GET: Admin/Contact
        public ActionResult Index(string Searchtext, int? page)
        {
            if (page == null)
            {
                page = 1;
            }
            var pageNumber = page ?? 1;
            var pageSize = 10;
            IEnumerable<tb_Contact> items = db.tb_Contact.OrderByDescending(x => x.CreatedDate).ToList();
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items= items.Where(x => x.UserName.Contains(Searchtext) || x.Email.Contains(Searchtext));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
           
           
          
            return View(items);
        }

        [HttpPost]
        public ActionResult IsRead(int id)
        {
            var item = db.tb_Contact.Find(id);
            if (item != null)
            {
                item.IsRead = !item.IsRead;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, IsRead = item.IsRead });
            }

            return Json(new { success = false });
        }
    }
}