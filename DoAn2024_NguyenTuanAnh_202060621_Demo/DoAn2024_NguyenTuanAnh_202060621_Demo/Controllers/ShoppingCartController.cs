using DoAn2024_NguyenTuanAnh_202060621_Demo.Models;
using DoAn2024_NguyenTuanAnh_202060621_Demo.Models.AutoAdd;
using DoAn2024_NguyenTuanAnh_202060621_Demo.Models.Cart;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DoAn2024_NguyenTuanAnh_202060621_Demo.Controllers
{
    public class ShoppingCartController : Controller
    {
        private WenBanHangOnline db = new WenBanHangOnline();       
        // GET: ShoppingCart
        public ActionResult Index()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
         
           
                if (cart != null && cart.Items.Any())
                {
                    ViewBag.CheckCart = cart;
                }
            
           
            return View();

        }
        public ActionResult OutOfStock()
        {
            return View();
        }
        public ActionResult Partial_Item_Cart()
        {
           
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart != null && cart.Items.Any())
                {
                    return PartialView(cart.Items);
                }
                return PartialView();
           

        }
        public ActionResult Partial_CheckOut()
        {
            //var user = UserManager.FindByNameAsync(User.Identity.Name).Result;
            //if (user != null)
            //{
            //    ViewBag.User = user;
            //}

            var user = (Models.AutoAdd.User)Session["user"];
            if (user != null)
            {
                ViewBag.User = user;
            }
            return PartialView();
            
          
          
        }
        [HttpPost]
        //[AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(OrderViewModel req)
        {
            var nhanVien = (Models.AutoAdd.User)Session["user"];
            ViewBag.nhanVien = nhanVien;
            if (nhanVien != null)
            {
                var code = new { Success = false, Code = -1, Url = "" };
            if (ModelState.IsValid)
            {
               
                    ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart != null)
                { 
                    
                        //User user = new User();
                        var user = db.Users.SingleOrDefault(m => m.UserName ==  nhanVien.UserName);
                    if (user != null)
                    {
                        tb_Order order = new tb_Order();
                        order.CustomerName = req.CustomerName;
                        order.Phone = req.Phone;
                        order.Address = req.Address;
                        order.Email = req.Email;
                        order.Status ="1";///chưa thanh toán / 2/đã thanh toán, 3/Hoàn thành, 4/hủy
                            cart.Items.ForEach(x => order.tb_OrderDetail.Add(new tb_OrderDetail
                        {
                            ProductId = x.ProductId,
                            Quantity = x.Quantity,
                            Price = x.Price
                        }));
                        order.Quantity = (int)cart.Items.Sum(x => (x.Quantity));
                        order.TotalAmount = cart.Items.Sum(x => (x.Price * x.Quantity));
                        order.TypePayment = req.TypePayment;
                        order.CreatedDate = DateTime.Now;
                        order.ModifiedDate = DateTime.Now;
                        order.CreatedBy = req.Phone;

                        

                        //if (User.Identity.IsAuthenticated)
                        //    order.CustomerId = User.Identity.GetUserId();
                        Random rd = new Random();
                        do
                        {
                            order.Code = "MaDH" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
                        } while (db.tb_Order.Any(o => o.Code == order.Code));
                            //order.E = req.CustomerName;
                            foreach (var item in cart.Items)
                            {
                                var product = db.tb_Product.SingleOrDefault(p => p.Id == item.ProductId);
                                if (product != null)
                                {
                                    // Kiểm tra xem số lượng sản phẩm đủ để bán không
                                    if (product.Quantity >= item.Quantity)
                                    {
                                        // Trừ đi số lượng sản phẩm đã mua từ tổng số lượng trong kho
                                        product.Quantity -= item.Quantity;
                                        // Cập nhật lại số lượng sản phẩm trong cơ sở dữ liệu
                                        db.Entry(product).State = EntityState.Modified;
                                        db.SaveChanges();
                                        db.tb_Order.Add(order);
                                        db.SaveChanges();
                                        var userOder = new UserOder
                                        {
                                            IdUser = user.Id,
                                            IdOder = order.Id,
                                        };
                                        db.UserOders.Add(userOder);
                                        db.SaveChanges();


                                    


                            var strSanPham = "";
                        var thanhtien = decimal.Zero;
                        var TongTien = decimal.Zero;
                                        var query = from o in db.tb_Order                                                   
                                                    join od in db.tb_OrderDetail
                                                    on o.Id equals od.OrderId
                                                    join p in db.tb_Product
                                                    on od.ProductId equals p.Id
                                                    select new
                        {
                          
                            ProductCode = p.ProductCode 
                        };
                        foreach (var sp in cart.Items)
                        {
                            var codesp = db.tb_Product.Where(x => x.Title == sp.ProductName).Select(x => x.ProductCode).FirstOrDefault(); 
                        
                            strSanPham += "<tr>";
                            strSanPham += "<td>" + sp.ProductName + "</td>";
                            strSanPham += "<td>" + codesp + "</td>";
                            strSanPham += "<td>" + sp.Quantity + "</td>";
                            strSanPham += "<td>" + DoAn2024_NguyenTuanAnh_202060621_Demo.Common.Common.FormatNumber(sp.TotalPrice, 0) +" ₫" + "</td>";
                            strSanPham += "</tr>";
                            thanhtien += sp.Price * sp.Quantity;
                        }
                        TongTien = thanhtien;
                        string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send2.html"));
                        contentCustomer = contentCustomer.Replace("{{MaDon}}", order.Code);
                        contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
                       
                        contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                        contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", order.CustomerName);
                        contentCustomer = contentCustomer.Replace("{{Phone}}", order.Phone);
                        contentCustomer = contentCustomer.Replace("{{Email}}", req.Email);
                        contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", order.Address);
                        contentCustomer = contentCustomer.Replace("{{ThanhTien}}", DoAn2024_NguyenTuanAnh_202060621_Demo.Common.Common.FormatNumber(thanhtien, 0));
                        contentCustomer = contentCustomer.Replace("{{TongTien}}", DoAn2024_NguyenTuanAnh_202060621_Demo.Common.Common.FormatNumber(TongTien, 0));
                        DoAn2024_NguyenTuanAnh_202060621_Demo.Common.Common.SendMail("TixCo", "Đơn hàng #" + order.Code, contentCustomer.ToString(), req.Email);

                        string contentAdmin = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send1.html"));
                        contentAdmin = contentAdmin.Replace("{{MaDon}}", order.Code);
                        contentAdmin = contentAdmin.Replace("{{SanPham}}", strSanPham);
                        contentAdmin = contentAdmin.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                        contentAdmin = contentAdmin.Replace("{{TenKhachHang}}", order.CustomerName);
                        contentAdmin = contentAdmin.Replace("{{Phone}}", order.Phone);
                        contentAdmin = contentAdmin.Replace("{{Email}}", req.Email);
                        contentAdmin = contentAdmin.Replace("{{DiaChiNhanHang}}", order.Address);
                        contentAdmin = contentAdmin.Replace("{{ThanhTien}}", DoAn2024_NguyenTuanAnh_202060621_Demo.Common.Common.FormatNumber(thanhtien, 0));
                        contentAdmin = contentAdmin.Replace("{{TongTien}}", DoAn2024_NguyenTuanAnh_202060621_Demo.Common.Common.FormatNumber(TongTien, 0));
                        DoAn2024_NguyenTuanAnh_202060621_Demo.Common.Common.SendMail("TixCo", "Đơn hàng mới #" + order.Code, contentAdmin.ToString(), ConfigurationManager.AppSettings["EmailAdmin"]);
                        cart.ClearCart();
                        return RedirectToAction("CheckOutSuccess");
                                    }
                                    else
                                    {
                                        return View("OutOfStock");
                                    }
                                }
                                else
                                {

                                }
                            }
                   

                    }
                    else
                    {
                        return RedirectToAction("DangNhap", "Home");
                    }

                }
               
            }

            return PartialView("Partial_CheckOut", req);
            }

            else
            {
                return RedirectToAction("DangNhap", "Home");
            }
        }
        [AllowAnonymous]
        public ActionResult ShowCount()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                return Json(new { Count = cart.Items.Count }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Count = 0 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Partial_Item_ThanhToan()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }

        public ActionResult CheckOut()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
            }
        
            return View();
        }

        public ActionResult CheckOutSuccess()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddToCart(int id, int quantity)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };

            var db = new WebBanHangOnline();
            var checkProduct = db.tb_Product.FirstOrDefault(x => x.Id == id);
            if (checkProduct != null)
            {
               
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart == null)
                {
                    cart = new ShoppingCart();
                }
                if (checkProduct.Quantity < quantity)
                {
                    // Trả về thông báo lỗi nếu số lượng sản phẩm trong giỏ hàng vượt quá số lượng có sẵn trong kho
                    code = new { Success = true , msg = "Số lượng sản phẩm trong kho không đủ!", code = -1, Count = 0 };
                    return Json(code);
                }
                ShoppingCartItem item = new ShoppingCartItem
                {
                    ProductId = checkProduct.Id,
                    ProductName = checkProduct.Title,
                    CategoryName = checkProduct.tb_ProductCategory.Title,
                    Alias = checkProduct.Alias,
                    Quantity = quantity
                };
                if (checkProduct.tb_ProductImage.FirstOrDefault(x => x.IsDefault) != null)
                {
                    item.ProductImg = checkProduct.tb_ProductImage.FirstOrDefault(x => x.IsDefault).Image;
                }
                item.Price = checkProduct.Price;
                if (checkProduct.PriceSale > 0)
                {
                    item.Price = (decimal)checkProduct.PriceSale;
                }
                item.TotalPrice = item.Quantity * item.Price;
                cart.AddToCart(item, quantity);
                Session["Cart"] = cart;
                code = new { Success = true, msg = "Thêm sản phẩm vào giở hàng thành công!", code = 1, Count = cart.Items.Count };
            }
            return Json(code);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };

            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                var checkProduct = cart.Items.FirstOrDefault(x => x.ProductId == id);
                if (checkProduct != null)
                {
                    cart.Remove(id);
                    code = new { Success = true, msg = "", code = 1, Count = cart.Items.Count };
                }
            }
            return Json(code);
        }
        [HttpPost]
        public ActionResult Update(int id, int quantity)
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                cart.UpdateQuantity(id, quantity);
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }
        [HttpPost]
        public ActionResult DeleteAll()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                cart.ClearCart();
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }
    }
}