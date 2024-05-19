using DoAn2024_NguyenTuanAnh_202060621_Demo.Models.AutoAdd;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;

namespace DoAn2024_NguyenTuanAnh_202060621_Demo.Controllers
{
    public class HomeController : Controller
    {
        private WenBanHangOnline db = new WenBanHangOnline();
       
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Introduce()
        {
            return View();
        }
        public ActionResult Partial_Subcrice()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Cancel(int id)
        {
            var item = db.tb_Order.Find(id);
            if (item != null)
            {
                item.Status = "5";
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }
        public ActionResult InformationUser()
        {
            var nhanVien = (Models.AutoAdd.User)Session["user"];
            if (nhanVien != null)
            {
                var user = db.Users.SingleOrDefault(m => m.UserName == nhanVien.UserName);
                if (user != null)
                {
                    var userOrders = db.UserOders.Where(m => m.IdUser == user.Id).ToList();
                    if (userOrders.Any())
                    {
                        List<tb_Order> orders = new List<tb_Order>();
                        List<tb_OrderDetail> orderDetails = new List<tb_OrderDetail>();
                        List<tb_Product> products = new List<tb_Product>();

                        foreach (var userOrder in userOrders)
                        {
                            var order = db.tb_Order.FirstOrDefault(x => x.Id == userOrder.IdOder);
                            if (order != null)
                            {
                                orders.Add(order);

                                var details = db.tb_OrderDetail.Where(x => x.OrderId == order.Id).ToList();
                                orderDetails.AddRange(details);

                                foreach (var detail in details)
                                {
                                    var product = db.tb_Product.FirstOrDefault(p => p.Id == detail.ProductId);
                                    if (product != null)
                                    {
                                        products.Add(product);
                                    }
                                }
                            }
                        }
                        orders = orders.OrderByDescending(o => o.CreatedDate).ToList();

                        ViewBag.User = user;
                        ViewBag.Orders = orders;
                        ViewBag.OrderDetails = orderDetails;
                        ViewBag.Products = products;

                        return View();
                    }
                    else
                    {
                        // Người dùng không có đơn hàng
                        ViewBag.User = user;
                        ViewBag.Orders = null;
                        ViewBag.OrderDetails = null;
                        ViewBag.Products = null;
                        return View();
                    }
                }
                else
                {
                    ViewBag.User = user;
                    ViewBag.Orders = null;
                    ViewBag.OrderDetails = null;
                    ViewBag.Products = null;
                    return View();
                }
            }
            else
            {
                return RedirectToAction("DangNhap", "Home");
            }

        }
        [HttpPost]
        public ActionResult Subscribe(tb_Subscribe req)
        {
            if (ModelState.IsValid)
            {
                db.tb_Subscribe.Add(new tb_Subscribe { Email = req.Email, CreatedDate = DateTime.Now });
                db.SaveChanges();
                return Json(new { Success = true });
            }
            return View("Partial_Subcrice", req);
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
            var nhanVien = dd.Users.SingleOrDefault(m => m.UserName.ToLower()==user.ToLower() && m.PassWord == hashedPassword);
            if (nhanVien != null)
            {
               
                Session["user"] = nhanVien;
                //ViewBag.user = nhanVien;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = "Tài khoản hoặc mật khẩu đăng nhập không đúng.";
                return View();
            }


        }
        public ActionResult DangXuat()
        {
            Session.Remove("user");
            Session.Remove("cart");
            FormsAuthentication.SignOut();
            return RedirectToAction("DangNhap", "Home");

        }
        public ActionResult DangKy()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKy([Bind(Include = "Id,Name,Address,Phone,Email,UserName,PassWord,ConfirmPassword")] User user)
        {
            var existingUser = db.Users.FirstOrDefault(u => u.UserName == user.UserName);

            if (existingUser != null)
            {
                ModelState.AddModelError("UserName", "Tài khoản đã tồn tại.");
            }
            if (ModelState.IsValid)
            {
                user.PassWord = GetMd5Hash(user.PassWord);
                user.ConfirmPassword = user.PassWord;
                //user.PassWord = BCrypt.Net.BCrypt.HashPassword(user.PassWord);
                db.Users.Add(user);
                db.SaveChanges();
                var userRole = new UserRole
                {
                    idUser = user.Id, // Sử dụng Id của User vừa tạo
                    idRole = 2 // Ví dụ: idRole là 1 (thay thế bằng idRole phù hợp trong ứng dụng của bạn)
                };
                db.UserRoles.Add(userRole);
                
                db.SaveChanges();
                return RedirectToAction("DangNhap","Home");
            }

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
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}