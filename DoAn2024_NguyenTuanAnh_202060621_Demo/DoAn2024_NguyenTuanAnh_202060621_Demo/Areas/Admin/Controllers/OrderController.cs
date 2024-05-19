using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using DoAn2024_NguyenTuanAnh_202060621_Demo.Models.AutoAdd;
using PagedList;

namespace DoAn2024_NguyenTuanAnh_202060621_Demo.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        private WenBanHangOnline db = new WenBanHangOnline();

        // GET: Admin/Order
        public ActionResult Index(string Searchtext, int? page)
        {
            //var items = db.tb_Order.OrderByDescending(x => x.CreatedDate).ToList();

            if (page == null)
            {
                page = 1;
            }
            var pageNumber = page ?? 1;
            var pageSize = 10;
            IEnumerable<tb_Order> items = db.tb_Order.OrderByDescending(x => x.CreatedDate).ToList();
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items= items.Where(x => x.Code.Contains(Searchtext) || x.CustomerName.Contains(Searchtext));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }

        public ActionResult View(int id)
        {
            var item = db.tb_Order.Find(id);
            return View(item);
        }
        public ActionResult Partial_SanPham(int id)
        {
            var items = db.tb_OrderDetail.Where(x => x.OrderId == id).ToList();
            return PartialView(items);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_Order tb_Order = db.tb_Order.Find(id);
            if (tb_Order == null)
            {
                return HttpNotFound();
            }
            var statusList = new List<SelectListItem>
    {
        new SelectListItem { Text = "Đã xác nhận", Value = "2" },
        new SelectListItem { Text = "Đã chuyển đơn hàng", Value = "3" },
        new SelectListItem { Text = "Hoàn thành", Value = "4" },
        new SelectListItem { Text = "Hủy đơn hàng", Value = "5" }
        // Thêm các trạng thái khác nếu cần
    };

            // Đặt danh sách lựa chọn vào ViewBag để truyền đến View
            ViewBag.StatusList = statusList;
            return View(tb_Order);
        }

        // POST: Admin/tb_Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Code,CustomerName,Status,Phone,Address,TotalAmount,Quantity,CreatedBy,CreatedDate,ModifiedDate,Modifiedby,TypePayment,Email")] tb_Order tb_Order)
        {

            if (ModelState.IsValid)
            {
                var nhanVien = (Models.AutoAdd.User)Session["user"];
                if (nhanVien != null)
                {
                    //User user = new User();
                    var user = db.Users.SingleOrDefault(m => m.UserName ==  nhanVien.UserName);
                    if (user != null)
                    { tb_Order.Modifiedby = user.UserName; }
                    else
                    {
                        tb_Order.Modifiedby = null;
                    }

                }
                else
                {
                    tb_Order.Modifiedby = null;
                }
                tb_Order.ModifiedDate = DateTime.Now;
                db.Entry(tb_Order).State = EntityState.Modified;
                db.SaveChanges();
                if (tb_Order.Status =="5")
                {
                    // Lấy danh sách các mặt hàng trong đơn hàng
                    var orderItems = db.tb_OrderDetail.Where(item => item.OrderId == tb_Order.Id).ToList();

                    // Cập nhật số lượng sản phẩm trong tb_Product
                    foreach (var item in orderItems)
                    {
                        var product = db.tb_Product.SingleOrDefault(p => p.Id == item.ProductId);
                        if (product != null)
                        {
                            // Cộng thêm số lượng sản phẩm đã đặt hàng vào số lượng hiện có trong kho
                            product.Quantity += item.Quantity;
                            db.Entry(product).State = EntityState.Modified;
                        }
                    }

                    // Lưu các thay đổi trong số lượng sản phẩm
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
        
            return View(tb_Order);
        }

    }
}
