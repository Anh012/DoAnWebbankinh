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
    public class NewsController : Controller
    {
        private WenBanHangOnline db = new WenBanHangOnline();
      
        // GET: Admin/News
        public ActionResult Index(string searchString)
        {
            var tb_News = db.tb_News.Include(t => t.tb_Category).OrderByDescending(x => x.CreatedDate);
            if (!String.IsNullOrEmpty(searchString))
            {
                tb_News= tb_News.Where(x => x.Title.Contains(searchString)).OrderByDescending(x => x.CreatedDate);
            }
            return View(tb_News.ToList());
        }

        // GET: Admin/News/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_News tb_News = db.tb_News.Find(id);
            if (tb_News == null)
            {
                return HttpNotFound();
            }
            return View(tb_News);
        }

        // GET: Admin/News/Create
        public ActionResult Create()
        {
          
            ViewBag.CategoryId = new SelectList(db.tb_Category, "Id", "Title");
            return View();
        }

        // POST: Admin/News/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Detail,Image,CategoryId,SeoTitle,SeoDescription,SeoKeywords,CreatedBy,CreatedDate,ModifiedDate,Modifiedby,Alias,IsActive")] tb_News tb_News)
        {
            if (ModelState.IsValid)
            {
                //string currentUserName = HttpContext.User.Identity.Name;
                //var currentUser = us.Users.FirstOrDefault(u => u.Name == currentUserName);

                //if (currentUser != null)
                //{
                //    // Gán giá trị của thuộc tính Name từ người dùng cho thuộc tính CreatedBy của tin tức
                //    tb_News.CreatedBy = currentUser.Name;
                //}
                //string currentUserName = Session["user"] as string;
                //tb_News.CreatedBy = currentUserName;
                var nhanVien = (Models.AutoAdd.User)Session["user"];
                if (nhanVien != null)
                {
                    //User user = new User();
                    var user = db.Users.SingleOrDefault(m => m.UserName ==  nhanVien.UserName);
                    if (user != null)
                    { tb_News.CreatedBy = user.UserName; }
                    else
                    {
                        tb_News.CreatedBy = null;
                    }

                }
                else
                {
                    tb_News.CreatedBy = null;
                }
                tb_News.Alias = DoAn2024_NguyenTuanAnh_202060621_Demo.Models.Common.Filter.FilterChar(tb_News.Title);
                tb_News.CreatedDate = DateTime.Now;
                tb_News.ModifiedDate = DateTime.Now;
                
                db.tb_News.Add(tb_News);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.tb_Category, "Id", "Title", tb_News.CategoryId);
            return View(tb_News);
        }

        // GET: Admin/News/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_News tb_News = db.tb_News.Find(id);
            if (tb_News == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.tb_Category, "Id", "Title", tb_News.CategoryId);
            return View(tb_News);
        }

        // POST: Admin/News/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Detail,Image,CategoryId,SeoTitle,SeoDescription,SeoKeywords,CreatedBy,CreatedDate,ModifiedDate,Modifiedby,Alias,IsActive")] tb_News tb_News)
        {
            if (ModelState.IsValid)
            {
                var nhanVien = (Models.AutoAdd.User)Session["user"];
                if (nhanVien != null)
                {
                    //User user = new User();
                    var user = db.Users.SingleOrDefault(m => m.UserName ==  nhanVien.UserName);
                    if (user != null)
                    { tb_News.Modifiedby = user.UserName; }
                    else
                    {
                        tb_News.Modifiedby = null;
                    }

                }
                else
                {
                    tb_News.Modifiedby = null;
                }
                tb_News.Alias = DoAn2024_NguyenTuanAnh_202060621_Demo.Models.Common.Filter.FilterChar(tb_News.Title);
                tb_News.ModifiedDate = DateTime.Now;
               
                db.Entry(tb_News).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.tb_Category, "Id", "Title", tb_News.CategoryId);
            return View(tb_News);
        }

        // GET: Admin/News/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_News tb_News = db.tb_News.Find(id);
            if (tb_News == null)
            {
                return HttpNotFound();
            }
            return View(tb_News);
        }

        // POST: Admin/News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tb_News tb_News = db.tb_News.Find(id);
            db.tb_News.Remove(tb_News);
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
