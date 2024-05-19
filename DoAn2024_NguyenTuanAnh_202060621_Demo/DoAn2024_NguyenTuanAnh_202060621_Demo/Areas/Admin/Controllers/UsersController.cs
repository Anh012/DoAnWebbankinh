using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DoAn2024_NguyenTuanAnh_202060621_Demo.App_Start;
using DoAn2024_NguyenTuanAnh_202060621_Demo.Models.AutoAdd;

using PagedList;

namespace DoAn2024_NguyenTuanAnh_202060621_Demo.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private WenBanHangOnline db = new WenBanHangOnline();
       
        [AdminAuthorize(IdChucNang = "1")]
        // GET: Admin/Users
        public ActionResult Index(int? page)
        {
            var items = db.Users.OrderBy(x => x.Id).ToList();
            if (page == null)
            {
                page = 1;
            }
            var pageNumber = page ?? 1;
            var pageSize = 10;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageNumber;
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Admin/Users/Create
        public ActionResult Create()
        {
            ViewBag.Roles = new SelectList(db.Roles, "Id", "Name");
           
            return View();
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Address,Name,Phone,Email,UserName,PassWord,ConfirmPassword")] User user, int roleId)
        {
            if (ModelState.IsValid)
            {
               
                if (db.Users.Any(u => u.UserName == user.UserName))
                {
                    ModelState.AddModelError("UserName", "Tên đăng nhập đã tồn tại.");
                    ViewBag.Roles = new SelectList(db.Roles, "Id", "Name", roleId);
                    return View(user);
                }
                user.PassWord = GetMd5Hash(user.PassWord);
                user.ConfirmPassword = user.PassWord;
                db.Users.Add(user);
                db.SaveChanges();
               
                var userRole = new UserRole
                {
                    idUser = user.Id, // Sử dụng Id của User vừa tạo
                    idRole = roleId // Sử dụng roleId từ form
                };
                db.UserRoles.Add(userRole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Roles = new SelectList(db.Roles, "Id", "Name", roleId);
            return View(user);
        }
        private string GetMd5Hash(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Chuyển đổi input thành mảng byte và băm
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Tạo một StringBuilder để lưu trữ kết quả băm
                StringBuilder sBuilder = new StringBuilder();

                // Chuyển đổi mỗi byte trong mảng đã băm thành một chuỗi hexa và thêm vào StringBuilder
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Trả về chuỗi hexa đã băm
                return sBuilder.ToString();
            }
        }
        // GET: Admin/Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.Roles = new SelectList(db.Roles, "Id", "Name");
            return View(user);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Phone,Email,UserName,PassWord,ConfirmPassword")] User user, int roleId)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
               
                var userRole = db.UserRoles.FirstOrDefault(u => u.idUser == user.Id);
              
                if (userRole != null)
                {
                    db.UserRoles.Remove(userRole);
                    db.SaveChanges();
                }
                
               
               
                // Tạo và lưu vai trò mới cho người dùng
                userRole = new UserRole
                {
                    idUser = user.Id,
                    idRole = roleId
                };
                db.UserRoles.Add(userRole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           
            ViewBag.Roles = new SelectList(db.Roles, "Id", "Name", roleId);
            return View(user);
        }

        // GET: Admin/Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            var userRole = db.UserRoles.FirstOrDefault(u => u.idUser == user.Id);
            if (userRole != null)
            {
                // Lấy tên của vai trò từ cơ sở dữ liệu
                var roleName = db.Roles.FirstOrDefault(r => r.Id == userRole.idRole)?.Name;

                // Đặt tên vai trò vào ViewBag để sử dụng trong View
                ViewBag.RoleName = roleName;
            }
            else
            {
                ViewBag.RoleName = "Không có vai trò"; // Nếu không tìm thấy vai trò
            }
            return View(user);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
