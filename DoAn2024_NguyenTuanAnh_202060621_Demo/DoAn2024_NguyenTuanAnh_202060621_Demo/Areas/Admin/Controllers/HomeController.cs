using DoAn2024_NguyenTuanAnh_202060621_Demo.App_Start;
using DoAn2024_NguyenTuanAnh_202060621_Demo.Models.AutoAdd;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DoAn2024_NguyenTuanAnh_202060621_Demo.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {

        //[Authorize]
        // GET: Admin/Home
        //public bool KiemTraChucNang(int idChucNang)
        //{
        //    DbUser dbb = new DbUser();
        //    User nvSession = (User)Session["user"];
        //    var count = dbb.UserRoles.Count(m => m.idUser == nvSession.Id && m.idRole == idChucNang);
        //    if (count == 0)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}

        //[AdminAuthorize(idChucNang = 1)]
        //[AdminAuthorize(IdChucNang = "1,3")]
        public ActionResult Index()
        {
            //if (KiemTraChucNang(3)== false)
            //{
            //    return Redirect("/Controllers/Home/Index");
            //}
            //if (Session["user"] == null)
            //{
            //    return RedirectToAction("DangNhap", "Home");
            //}
            //else
            //{
                return View();
            //}
            
        }
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(string user, string password) 
        {
            WenBanHangOnline dd = new WenBanHangOnline();
            string hashedPassword = GetMd5Hash(password);
            var nhanVien = dd.Users.SingleOrDefault (m=>m.UserName.ToLower()==user.ToLower() && m.PassWord == hashedPassword);
            if (nhanVien != null)
            {
                Session["user"] = nhanVien;
                //ViewBag.user = nhanVien;
                return RedirectToAction("Index","Home");
            }
            else
            {
                TempData["error"] = "Tài khoản đăng nhập không đúng.";
                return View();
            }

            
        }
        public ActionResult DangXuat()
        {
            Session.Remove("user");
            FormsAuthentication.SignOut();
            return RedirectToAction("DangNhap", "Home");
                
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
    }
}