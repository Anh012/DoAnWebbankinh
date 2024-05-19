using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn2024_NguyenTuanAnh_202060621_Demo.Models.AutoAdd;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace DoAn2024_NguyenTuanAnh_202060621_Demo.Areas.Admin.Controllers
{
    public class StatisticalController : Controller
    {
        private WenBanHangOnline db = new WenBanHangOnline();
        // GET: Admin/Statistical
        public ActionResult Index()
        {

            return View(db.tb_Product.ToList());
        }
        [HttpGet]
        public ActionResult GetStatistical(string fromDate, string toDate)
        {
            var query = from o in db.tb_Order
                        where o.Status != "5"
                        join od in db.tb_OrderDetail
                        on o.Id equals od.OrderId
                        join p in db.tb_Product
                        on od.ProductId equals p.Id
                        select new
                        {
                            CreatedDate = o.CreatedDate,
                            Quantity = od.Quantity,
                            Price = od.Price,
                            OriginalPrice = p.Price * 0.7m,
                            ProductName = p.Title 
                        };

            if (!string.IsNullOrEmpty(fromDate))
            {
                if (DateTime.TryParseExact(fromDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime startDate))
                {
                    query = query.Where(x => x.CreatedDate >= startDate);
                }
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                if (DateTime.TryParseExact(toDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime endDate))
                {
                    query = query.Where(x => x.CreatedDate < endDate);
                }
            }

            var result = query.GroupBy(x => DbFunctions.TruncateTime(x.CreatedDate)).Select(x => new
            {
                Date = x.Key.Value,
                TotalBuy = x.Sum(y => y.Quantity * y.OriginalPrice),
                TotalSell = x.Sum(y => y.Quantity * y.Price),
                ProductSales = x.GroupBy(y => y.ProductName).Select(y => new
                {
                    ProductName = y.Key,
                    QuantitySold = y.Sum(z => z.Quantity)
                }).ToList()
            }).Select(x => new
            {
                Date = x.Date,
                DoanhThu = x.TotalSell,
                LoiNhuan = x.TotalSell - x.TotalBuy,
                ProductSales = x.ProductSales
            }).ToList();

            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetProductSales(string fromDate, string toDate)
        {
            var querys = from o in db.tb_Order
                        where o.Status != "5"
                        join od in db.tb_OrderDetail
                        on o.Id equals od.OrderId
                        join p in db.tb_Product
                        on od.ProductId equals p.Id
                        select new
                        {
                            CreatedDate = o.CreatedDate,                         
                        };
            var query = from o in db.tb_Order  where o.Status != "5"
                        join od in db.tb_OrderDetail on o.Id equals od.OrderId
                        join p in db.tb_Product on od.ProductId equals p.Id
                        group od by p.Title into g
                        select new
                        {
                           
                            ProductName = g.Key,
                            QuantitySold = g.Sum(od => od.Quantity)
                        };
            if (!string.IsNullOrEmpty(fromDate))
            {
                if (DateTime.TryParseExact(fromDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime startDate))
                {
                    querys = querys.Where(x => x.CreatedDate >= startDate);
                }
                //DateTime startDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
                //query = query.Where(x => x.CreatedDate >= startDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                if (DateTime.TryParseExact(toDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime endDate))
                {
                    querys = querys.Where(x => x.CreatedDate < endDate);
                }
                //DateTime endDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);
                //query = query.Where(x => x.CreatedDate < endDate);
            }
            var result = query.ToList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

    }
}