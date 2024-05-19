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
    public class PostsController : Controller
    {
        private WenBanHangOnline db = new WenBanHangOnline();

        // GET: Admin/Posts
        public ActionResult Index(string searchString)
        {
            var tb_Posts = db.tb_Posts.Include(t => t.tb_Category);
            if (!String.IsNullOrEmpty(searchString))
            {
                tb_Posts= tb_Posts.Where(x => x.Title.Contains(searchString));
            }
            return View(tb_Posts.ToList());
        }

        // GET: Admin/Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_Posts tb_Posts = db.tb_Posts.Find(id);
            if (tb_Posts == null)
            {
                return HttpNotFound();
            }
            return View(tb_Posts);
        }

        // GET: Admin/Posts/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.tb_Category, "Id", "Title");
            return View();
        }

        // POST: Admin/Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Detail,Image,CategoryId,SeoTitle,SeoDescription,SeoKeywords,CreatedBy,CreatedDate,ModifiedDate,Modifiedby,Alias,IsActive")] tb_Posts tb_Posts)
        {
            if (ModelState.IsValid)
            {
                var nhanVien = (Models.AutoAdd.User)Session["user"];
                if (nhanVien != null)
                {
                    //User user = new User();
                    var user = db.Users.SingleOrDefault(m => m.UserName ==  nhanVien.UserName);
                    if (user != null)
                    { tb_Posts.CreatedBy = user.UserName; }
                    else
                    {
                        tb_Posts.CreatedBy = null;
                    }

                }
                else
                {
                    tb_Posts.CreatedBy = null;
                }
                tb_Posts.Alias = DoAn2024_NguyenTuanAnh_202060621_Demo.Models.Common.Filter.FilterChar(tb_Posts.Title);
                tb_Posts.CreatedDate = DateTime.Now;
                tb_Posts.ModifiedDate = DateTime.Now;
                db.tb_Posts.Add(tb_Posts);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.tb_Category, "Id", "Title", tb_Posts.CategoryId);
            return View(tb_Posts);
        }

        // GET: Admin/Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_Posts tb_Posts = db.tb_Posts.Find(id);
            if (tb_Posts == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.tb_Category, "Id", "Title", tb_Posts.CategoryId);
            return View(tb_Posts);
        }

        // POST: Admin/Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Detail,Image,CategoryId,SeoTitle,SeoDescription,SeoKeywords,CreatedBy,CreatedDate,ModifiedDate,Modifiedby,Alias,IsActive")] tb_Posts tb_Posts)
        {
            if (ModelState.IsValid)
            {
                var nhanVien = (Models.AutoAdd.User)Session["user"];
                if (nhanVien != null)
                {
                    //User user = new User();
                    var user = db.Users.SingleOrDefault(m => m.UserName ==  nhanVien.UserName);
                    if (user != null)
                    { tb_Posts.Modifiedby = user.UserName; }
                    else
                    {
                        tb_Posts.Modifiedby = null;
                    }

                }
                else
                {
                    tb_Posts.Modifiedby = null;
                }
                tb_Posts.Alias = DoAn2024_NguyenTuanAnh_202060621_Demo.Models.Common.Filter.FilterChar(tb_Posts.Title);
                tb_Posts.ModifiedDate = DateTime.Now;
                db.Entry(tb_Posts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.tb_Category, "Id", "Title", tb_Posts.CategoryId);
            return View(tb_Posts);
        }

        // GET: Admin/Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_Posts tb_Posts = db.tb_Posts.Find(id);
            if (tb_Posts == null)
            {
                return HttpNotFound();
            }
            return View(tb_Posts);
        }

        // POST: Admin/Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tb_Posts tb_Posts = db.tb_Posts.Find(id);
            db.tb_Posts.Remove(tb_Posts);
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
