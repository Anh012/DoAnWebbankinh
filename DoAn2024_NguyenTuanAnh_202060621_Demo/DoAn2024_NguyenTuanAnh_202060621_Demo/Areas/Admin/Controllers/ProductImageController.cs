using DoAn2024_NguyenTuanAnh_202060621_Demo.Models.AutoAdd;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn2024_NguyenTuanAnh_202060621_Demo.Areas.Admin.Controllers
{
    public class ProductImageController : Controller
    {
        private WenBanHangOnline db = new WenBanHangOnline();
        // GET: Admin/ProductImage
        public ActionResult Index(int id)
        {
            ViewBag.ProductId = id;
            var items = db.tb_ProductImage.Where(x => x.ProductId == id).ToList();
            return PartialView(items);
            
        }

        [HttpPost]
        public ActionResult AddImage(int productId, string url)
        {
            db.tb_ProductImage.Add(new tb_ProductImage
            {
                ProductId=productId,
                Image=url,
                IsDefault=false
            });
            db.SaveChanges();
            return Json(new { Success = true });
        }
        [HttpPost]
        public ActionResult DeleteImage(int imageId)
        {
            var imageToDelete = db.tb_ProductImage.Find(imageId);
            if (imageToDelete != null)
            {
                // Xóa ảnh khỏi cơ sở dữ liệu
                db.tb_ProductImage.Remove(imageToDelete);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        public ActionResult UpdateIsDefault(int imageId, bool isDefault)
        {
            var image = db.tb_ProductImage.Find(imageId);
            if (image != null)
            {
                image.IsDefault = isDefault;
                db.Entry(image).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}