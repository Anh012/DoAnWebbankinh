using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAn2024_NguyenTuanAnh_202060621_Demo.Models.AutoAdd;

namespace DoAn2024_NguyenTuanAnh_202060621_Demo.Areas.Admin.Controllers
{
    public class ProductCategoryController : Controller
    {
        private WenBanHangOnline db = new WenBanHangOnline();

        // GET: Admin/ProductCategory
        public ActionResult Index()
        {
            return View(db.tb_ProductCategory.ToList());
        }

        // GET: Admin/ProductCategory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_ProductCategory tb_ProductCategory = db.tb_ProductCategory.Find(id);
            if (tb_ProductCategory == null)
            {
                return HttpNotFound();
            }
            return View(tb_ProductCategory);
        }

        // GET: Admin/ProductCategory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/ProductCategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Icon,SeoTitle,SeoDescription,SeoKeywords,CreatedBy,CreatedDate,ModifiedDate,Modifiedby,Alias")] tb_ProductCategory tb_ProductCategory)
        {
            if (ModelState.IsValid)
            {
                var nhanVien = (Models.AutoAdd.User)Session["user"];
                if (nhanVien != null)
                {
                    //User user = new User();
                    var user = db.Users.SingleOrDefault(m => m.UserName ==  nhanVien.UserName);
                    if (user != null)
                    { tb_ProductCategory.CreatedBy = user.UserName; }
                    else
                    {
                        tb_ProductCategory.CreatedBy = null;
                    }

                }
                else
                {
                    tb_ProductCategory.CreatedBy = null;
                }
                tb_ProductCategory.Alias = DoAn2024_NguyenTuanAnh_202060621_Demo.Models.Common.Filter.FilterChar(tb_ProductCategory.Title);
                tb_ProductCategory.CreatedDate = DateTime.Now;
                tb_ProductCategory.ModifiedDate = DateTime.Now;
                db.tb_ProductCategory.Add(tb_ProductCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tb_ProductCategory);
        }

        // GET: Admin/ProductCategory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_ProductCategory tb_ProductCategory = db.tb_ProductCategory.Find(id);
            if (tb_ProductCategory == null)
            {
                return HttpNotFound();
            }
            return View(tb_ProductCategory);
        }

        // POST: Admin/ProductCategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Icon,SeoTitle,SeoDescription,SeoKeywords,CreatedBy,CreatedDate,ModifiedDate,Modifiedby,Alias")] tb_ProductCategory tb_ProductCategory)
        {
            if (ModelState.IsValid)
            {
                var nhanVien = (Models.AutoAdd.User)Session["user"];
                if (nhanVien != null)
                {
                    //User user = new User();
                    var user = db.Users.SingleOrDefault(m => m.UserName ==  nhanVien.UserName);
                    if (user != null)
                    { tb_ProductCategory.Modifiedby = user.UserName; }
                    else
                    {
                        tb_ProductCategory.Modifiedby = null;
                    }

                }
                else
                {
                    tb_ProductCategory.Modifiedby = null;
                }
                tb_ProductCategory.Alias = DoAn2024_NguyenTuanAnh_202060621_Demo.Models.Common.Filter.FilterChar(tb_ProductCategory.Title);
                tb_ProductCategory.ModifiedDate = DateTime.Now;
                db.Entry(tb_ProductCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tb_ProductCategory);
        }

        // GET: Admin/ProductCategory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_ProductCategory tb_ProductCategory = db.tb_ProductCategory.Find(id);
            if (tb_ProductCategory == null)
            {
                return HttpNotFound();
            }
            return View(tb_ProductCategory);
        }

        // POST: Admin/ProductCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tb_ProductCategory tb_ProductCategory = db.tb_ProductCategory.Find(id);
            db.tb_ProductCategory.Remove(tb_ProductCategory);
            db.SaveChanges();
            return RedirectToAction("Index");
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
