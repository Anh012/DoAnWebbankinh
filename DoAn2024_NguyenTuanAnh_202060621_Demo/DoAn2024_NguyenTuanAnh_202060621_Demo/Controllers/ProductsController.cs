using DoAn2024_NguyenTuanAnh_202060621_Demo.Models.AutoAdd;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn2024_NguyenTuanAnh_202060621_Demo.Controllers
{
    public class ProductsController : Controller
    {
        private WenBanHangOnline db = new WenBanHangOnline();
        // GET: Products
        public ActionResult HienThi(string Searchtext, int? page)
        {
            
            if (page == null)
            {
                page = 1;
            }
            var pageNumber = page ?? 1;
            var pageSize = 8;
            IEnumerable<tb_Product> items = db.tb_Product.Where(x => x.IsActive).ToList();
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items= items.Where(x => x.Title.Contains(Searchtext) || x.ProductCode.Contains(Searchtext));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public ActionResult Index()
        {
            var items = db.tb_Product.ToList();
            return View(items);
        }
        public ActionResult SpNew()
        {
            var items = db.tb_Product.Where(x => x.IsHome && x.IsActive && x.IsFeature).ToList();
            return PartialView(items);
        }
        public ActionResult SpSale()
        {
            var items = db.tb_Product.Where(x => x.IsActive && x.IsSale).ToList();
            return PartialView(items);
        }
        public ActionResult SpHot()
        {
            var items = db.tb_Product.Where(x => x.IsActive && x.IsHome && x.IsHot).ToList();
            return PartialView(items);
        }
        public ActionResult ProductCategory(string alias, int id, string Searchtext, int? page)
        {
            if (page == null)
            {
                page = 1;
            }
            var pageSize = 8;

            var items = db.tb_Product.Where(x => x.IsActive);

            if (id > 0)
            {
                items = items.Where(x => x.ProductCategoryId == id);
            }

            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Title.Contains(Searchtext) || x.ProductCode.Contains(Searchtext));
            }

            // Sắp xếp dữ liệu trước khi áp dụng phân trang
            items = items.OrderBy(x => x.Id);

            var paginatedItems = items.ToPagedList(page ?? 1, pageSize);

            var cate = db.tb_ProductCategory.Find(id);
            if (cate != null)
            {
                ViewBag.CateName = cate.Title;
            }

            ViewBag.CateId = id;
            ViewBag.Searchtext = Searchtext;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            //var items = db.tb_Product.ToList();
            //if (id > 0)
            //{
            //    items = items.Where(x => x.ProductCategoryId == id).ToList();
            //}
            //var cate = db.tb_ProductCategory.Find(id);
            //if (cate != null)
            //{
            //    ViewBag.CateName = cate.Title;
            //}

            //ViewBag.CateId = id;
            return View(paginatedItems);
        }
        public ActionResult ChiTiet(string alias, int id)
        {
            var item = db.tb_Product.Find(id);
            if (item != null)
            {
                db.tb_Product.Attach(item);
                item.ViewCount = item.ViewCount + 1;
                db.Entry(item).Property(x => x.ViewCount).IsModified = true;
                db.SaveChanges();
            }
            var nhanVien = (Models.AutoAdd.User)Session["user"];
            
            if (nhanVien != null)
            {
                bool hasOrder = (from x in db.UserOders
                                 join o in db.tb_Order on x.IdOder equals o.Id
                                 join od in db.tb_OrderDetail on o.Id equals od.OrderId
                                 where x.IdUser == nhanVien.Id && od.ProductId == id && o.Status == "4"
                                 select o).Any();

                ViewBag.NhanVienExists = hasOrder;
            }
            else
            {
                ViewBag.NhanVienExists = null;
            }
           

            var itemm = db.tb_Reviews.Where(x => x.ProductId == id).OrderByDescending(x => x.Id).ToList();
            var intRates = itemm.Select(x => int.Parse(x.Rate));
            ViewBag.Count = itemm.Count;

            ViewBag.Sum = intRates.Sum();
            if (ViewBag.Count > 0)
            {
                ViewBag.Average = ViewBag.Sum / ViewBag.Count;
            }
            else
            {
                ViewBag.Average = 0; // Tránh chia cho 0 nếu không có đánh giá
            }
            //var countReview = db.Reviews.Where(x => x.ProductId == id).Count();
            //ViewBag.CountReview = countReview;
            return View(item);
        }
        public ActionResult HCN(string Searchtext, int? page)
        {
            if (page == null)
            {
                page = 1;
            }
            var pageNumber = page ?? 1;
            var pageSize = 8;
            IEnumerable<tb_Product> items = db.tb_Product.Where(x => x.ProductCode.Substring(1).StartsWith("CN")&& x.IsActive).ToList();
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items= items.Where(x => x.Title.Contains(Searchtext) || x.ProductCode.Contains(Searchtext));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            
            return View(items);
        }
        public ActionResult Oval(string Searchtext, int? page)
        {
            if (page == null)
            {
                page = 1;
            }
            var pageNumber = page ?? 1;
            var pageSize = 8;
            IEnumerable<tb_Product> items = db.tb_Product.Where(x => x.ProductCode.Substring(1).StartsWith("OV")&& x.IsActive).ToList();
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items= items.Where(x => x.Title.Contains(Searchtext) || x.ProductCode.Contains(Searchtext));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public ActionResult Tron(string Searchtext, int? page)
        {
            if (page == null)
            {
                page = 1;
            }
            var pageNumber = page ?? 1;
            var pageSize = 8;
            IEnumerable<tb_Product> items = db.tb_Product.Where(x => x.ProductCode.Substring(1).StartsWith("T")&& x.IsActive).ToList();
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items= items.Where(x => x.Title.Contains(Searchtext) || x.ProductCode.Contains(Searchtext));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public ActionResult Price1(string Searchtext, int? page)
        {
            if (page == null)
            {
                page = 1;
            }
            var pageNumber = page ?? 1;
            var pageSize = 8;
            IEnumerable<tb_Product> items = db.tb_Product.Where(x => x.Price <= 1000000  && x.IsActive).ToList();
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items= items.Where(x => x.Title.Contains(Searchtext) || x.ProductCode.Contains(Searchtext));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
        
            return View(items);
        }
        public ActionResult Price2(string Searchtext, int? page)
        {
            if (page == null)
            {
                page = 1;
            }
            var pageNumber = page ?? 1;
            var pageSize = 8;
            IEnumerable<tb_Product> items = db.tb_Product.Where(x => x.Price >= 1000000 && x.Price <= 2000000 && x.IsActive).ToList();
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items= items.Where(x => x.Title.Contains(Searchtext) || x.ProductCode.Contains(Searchtext));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
          
            return View(items);
        }
        public ActionResult Price3(string Searchtext, int? page)
        {
            if (page == null)
            {
                page = 1;
            }
            var pageNumber = page ?? 1;
            var pageSize = 8;
            IEnumerable<tb_Product> items = db.tb_Product.Where(x => x.Price >= 2000000&& x.IsActive).ToList();
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items= items.Where(x => x.Title.Contains(Searchtext) || x.ProductCode.Contains(Searchtext));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
           
            return View(items);
        }
        //public ActionResult Detail(string alias, int id)
        //{
        //    var item = db.tb_Product.Find(id);
        //    if (item != null)
        //    {
        //        db.tb_Product.Attach(item);
        //        item.ViewCount = item.ViewCount + 1;
        //        db.Entry(item).Property(x => x.ViewCount).IsModified = true;
        //        db.SaveChanges();
        //    }
        //    //var countReview = db.Reviews.Where(x => x.ProductId == id).Count();
        //    //ViewBag.CountReview = countReview;
        //    return View(item);
        //}
    }
}