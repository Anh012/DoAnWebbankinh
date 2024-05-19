using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAn2024_NguyenTuanAnh_202060621_Demo.Models;
using DoAn2024_NguyenTuanAnh_202060621_Demo.Models.AutoAdd;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace DoAn2024_NguyenTuanAnh_202060621_Demo.Controllers
{
    public class tb_ReviewController : Controller
    {
        private WenBanHangOnline db = new WenBanHangOnline();

        // GET: tb_Review
        public ActionResult Index()
        {
            return View(db.tb_Reviews.ToList());
        }

        public ActionResult _Review(int productId)
        {
            ViewBag.ProductId = productId;
            var item = new tb_Review();
            var nhanVien = (Models.AutoAdd.User)Session["user"];
            if (nhanVien != null)
            {
                var user = db.Users.SingleOrDefault(m => m.UserName == nhanVien.UserName);
                if (user != null)
                {
                    item.Emaill = user.Email;
                    item.FullName = user.Name;
                    item.UserName = user.UserName;
                }
                return PartialView(item);
            }
            return PartialView();
        }
        public ActionResult _Load_Review(int productId)
        {
            var item = db.tb_Reviews.Where(x => x.ProductId == productId).OrderByDescending(x => x.Id).ToList();
            ViewBag.Count = item.Count;
            return PartialView(item);
        }
        [HttpPost]
        public ActionResult PostReview(tb_Review req)
        {
            
            
            if (ModelState.IsValid)
            {
                req.CreatedDate = DateTime.Now;
                db.tb_Reviews.Add(req);
                db.SaveChanges();
                return Json(new { Success = true });
            }
           
            return Json(new { Success = false });
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
