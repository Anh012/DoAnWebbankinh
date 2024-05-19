using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAn2024_NguyenTuanAnh_202060621_Demo.Models.AutoAdd;

using Microsoft.Ajax.Utilities;
using DoAn2024_NguyenTuanAnh_202060621_Demo.App_Start;
namespace DoAn2024_NguyenTuanAnh_202060621_Demo.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private WenBanHangOnline db = new WenBanHangOnline();

        //public bool KiemTraChucNang(int idChucNang)
        //{
        //    DbUser dbb = new DbUser();
        //    User nvSession = (User)Session["user"];
        //    var count = dbb.UserRoles.Count(m => m.idUser == nvSession.Id && m.idRole == idChucNang);
        //    if (count == 0)
        //    {
        //        return false;
        //    }else
        //    {
        //        return true;
        //    }
        //} 
        //if (KiemTraChucNang(1)== false)
        //{
        //   return Redirect("/Admin/Error/KhongCoQuyen");
        //}
        // GET: Admin/Category
        [AdminAuthorize(IdChucNang = "1,3")]

        public ActionResult Index()
        {
           
            return View(db.tb_Category.ToList());
        }

        // GET: Admin/Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_Category tb_Category = db.tb_Category.Find(id);
            if (tb_Category == null)
            {
                return HttpNotFound();
            }
            return View(tb_Category);
        }

        // GET: Admin/Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Position,CreatedBy,CreatedDate,ModifiedDate,Modifiedby,Alias,IsActive")] tb_Category tb_Category)
        {
            if (ModelState.IsValid)
            {
                var nhanVien = (Models.AutoAdd.User)Session["user"];
                if (nhanVien != null)
                {
                    //User user = new User();
                    var user = db.Users.SingleOrDefault(m => m.UserName ==  nhanVien.UserName);
                    if (user != null)
                    { tb_Category.CreatedBy = user.UserName; }
                    else
                    {
                        tb_Category.CreatedBy = null;
                    }

                }
                else
                {
                    tb_Category.CreatedBy = null;
                }
                tb_Category.Alias = DoAn2024_NguyenTuanAnh_202060621_Demo.Models.Common.Filter.FilterChar(tb_Category.Title);
                tb_Category.CreatedDate = DateTime.Now;
                tb_Category.ModifiedDate = DateTime.Now;
                db.tb_Category.Add(tb_Category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tb_Category);
        }

        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_Category tb_Category = db.tb_Category.Find(id);
            if (tb_Category == null)
            {
                return HttpNotFound();
            }
            return View(tb_Category);
        }

        // POST: Admin/Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Position,CreatedBy,CreatedDate,ModifiedDate,Modifiedby,Alias,IsActive")] tb_Category tb_Category)
        {
            if (ModelState.IsValid)
            {

                var nhanVien = (Models.AutoAdd.User)Session["user"];
                if (nhanVien != null)
                {
                    //User user = new User();
                    var user = db.Users.SingleOrDefault(m => m.UserName ==  nhanVien.UserName);
                    if (user != null)
                    { tb_Category.Modifiedby = user.UserName; }
                    else
                    {
                        tb_Category.Modifiedby = null;
                    }

                }
                else
                {
                    tb_Category.Modifiedby = null;
                }
                tb_Category.Alias = DoAn2024_NguyenTuanAnh_202060621_Demo.Models.Common.Filter.FilterChar(tb_Category.Title);
                tb_Category.ModifiedDate = DateTime.Now;
                db.Entry(tb_Category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tb_Category);
        }

        // GET: Admin/Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_Category tb_Category = db.tb_Category.Find(id);
            if (tb_Category == null)
            {
                return HttpNotFound();
            }
            return View(tb_Category);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tb_Category tb_Category = db.tb_Category.Find(id);
            db.tb_Category.Remove(tb_Category);
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
