using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAn2024_NguyenTuanAnh_202060621_Demo.Models.AutoAdd;
using PagedList;

namespace DoAn2024_NguyenTuanAnh_202060621_Demo.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private WenBanHangOnline db = new WenBanHangOnline();

        // GET: Admin/Product
        public ActionResult Index(string Searchtext, int? page)
        {
            if (page == null)
            {
                page = 1;
            }
            var pageNumber = page ?? 1;
            var pageSize = 10;
            IEnumerable<tb_Product> items = db.tb_Product.OrderByDescending(x => x.CreatedDate).Include(t => t.tb_ProductCategory);
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

        // GET: Admin/Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_Product tb_Product = db.tb_Product.Find(id);
            if (tb_Product == null)
            {
                return HttpNotFound();
            }
            return View(tb_Product);
        }

        // GET: Admin/Product/Create
        public ActionResult Create()
        {
            ViewBag.ProductCategoryId = new SelectList(db.tb_ProductCategory, "Id", "Title");
            return View();
        }

        // POST: Admin/Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,ProductCode,Description,Detail,Image,Price,PriceSale,Quantity,IsHome,IsSale,IsFeature,IsHot,ProductCategoryId,SeoTitle,SeoDescription,SeoKeywords,CreatedBy,CreatedDate,ModifiedDate,Modifiedby,Alias,IsActive,ViewCount")] tb_Product tb_Product, List<string> Images, List<int> rDefault)
        {
            if (ModelState.IsValid)
            {
                var nhanVien = (Models.AutoAdd.User)Session["user"];
                if (nhanVien != null)
                {
                    //User user = new User();
                    var user = db.Users.SingleOrDefault(m => m.UserName ==  nhanVien.UserName);
                    if (user != null)
                    { tb_Product.CreatedBy = user.UserName; }
                    else
                    {
                        tb_Product.CreatedBy = null;
                    }

                }
                else
                {
                    tb_Product.CreatedBy = null;
                }
                if (Images != null && Images.Count > 0)
                {
                    for (int i = 0; i < Images.Count; i++)
                    {
                        if (i + 1 == rDefault[0])
                        {
                            tb_Product.Image = Images[i];
                            tb_Product.tb_ProductImage.Add(new tb_ProductImage
                            {
                                ProductId = tb_Product.Id,
                                Image = Images[i],
                                IsDefault = true
                            });
                        }
                        else
                        {
                            tb_Product.tb_ProductImage.Add(new tb_ProductImage
                            {
                                ProductId = tb_Product.Id,
                                Image = Images[i],
                                IsDefault = false
                            });
                        }
                    }
                }
                tb_Product.Alias = DoAn2024_NguyenTuanAnh_202060621_Demo.Models.Common.Filter.FilterChar(tb_Product.Title);
                tb_Product.CreatedDate = DateTime.Now;
                tb_Product.ModifiedDate = DateTime.Now;
                if (string.IsNullOrEmpty(tb_Product.SeoTitle))
                {
                    tb_Product.SeoTitle = tb_Product.Title;
                }
                db.tb_Product.Add(tb_Product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductCategoryId = new SelectList(db.tb_ProductCategory, "Id", "Title", tb_Product.ProductCategoryId);
            return View(tb_Product);
        }

        // GET: Admin/Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_Product tb_Product = db.tb_Product.Find(id);
            if (tb_Product == null)
            {
                return HttpNotFound();
            }
            var productImages = db.tb_ProductImage.Where(x => x.ProductId == id).ToList();
            ViewBag.ProductImages = productImages;
            ViewBag.ProductCategoryId = new SelectList(db.tb_ProductCategory, "Id", "Title", tb_Product.ProductCategoryId);
            return View(tb_Product);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,ProductCode,Description,Detail,Image,Price,PriceSale,Quantity,IsHome,IsSale,IsFeature,IsHot,ProductCategoryId,SeoTitle,SeoDescription,SeoKeywords,CreatedBy,CreatedDate,ModifiedDate,Modifiedby,Alias,IsActive,ViewCount")] tb_Product tb_Product, int[] deletedImageIds, List<string> Images, List<int> rDefault, int? defaultImageId, List<tb_ProductImage> productImages)
        {
            if (ModelState.IsValid)
            {
                var nhanVien = (Models.AutoAdd.User)Session["user"];
                if (nhanVien != null)
                {
                    //User user = new User();
                    var user = db.Users.SingleOrDefault(m => m.UserName ==  nhanVien.UserName);
                    if (user != null)
                    { tb_Product.Modifiedby = user.UserName; }
                    else
                    {
                        tb_Product.Modifiedby = null;
                    }

                }
                else
                {
                    tb_Product.Modifiedby = null;
                }             
                if (Images != null && Images.Count > 0)
                {
                    for (int i = 0; i < Images.Count; i++)
                    {
                        var productImage = new tb_ProductImage
                        {
                            ProductId = tb_Product.Id,
                            Image = Images[i],
                            IsDefault = (i + 1 == rDefault[0]) // Set IsDefault based on rDefault
                        };

                        // Set IsDefault to false if it's not the default image
                        if (!productImage.IsDefault)
                        {
                            productImage.IsDefault = false;
                        }

                        tb_Product.tb_ProductImage.Add(productImage);
                        db.tb_ProductImage.Add(productImage);
                    }
                }
                tb_Product.Alias = DoAn2024_NguyenTuanAnh_202060621_Demo.Models.Common.Filter.FilterChar(tb_Product.Title);
                tb_Product.ModifiedDate = DateTime.Now;
                if (deletedImageIds != null)
                {
                    foreach (var imageId in deletedImageIds)
                    {
                        var imageToDelete = db.tb_ProductImage.Find(imageId);
                        if (imageToDelete != null)
                        {
                            db.tb_ProductImage.Remove(imageToDelete);
                        }
                    }
                }
                db.Entry(tb_Product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductCategoryId = new SelectList(db.tb_ProductCategory, "Id", "Title", tb_Product.ProductCategoryId);
            return View(tb_Product);
        }

        // GET: Admin/Product/Delete/5
        public ActionResult Delete(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_Product tb_Product = db.tb_Product.Find(id);
            if (tb_Product == null)
            {
                return HttpNotFound();
            }
            var CheckImage = db.tb_ProductImage.FirstOrDefault(x => x.ProductId == id && x.IsDefault == true);

            if (CheckImage != null)
            {
                ViewBag.strImg = CheckImage.Image;
            }
            return View(tb_Product);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tb_Product tb_Product = db.tb_Product.Find(id);
            db.tb_Product.Remove(tb_Product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = db.tb_Product.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, isAcive = item.IsActive });
            }

            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult IsHome(int id)
        {
            var item = db.tb_Product.Find(id);
            if (item != null)
            {
                item.IsHome = !item.IsHome;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, IsHome = item.IsHome });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsSale(int id)
        {
            var item = db.tb_Product.Find(id);
            if (item != null)
            {
                item.IsSale = !item.IsSale;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, IsSale = item.IsSale });
            }

            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult IsHot(int id)
        {
            var item = db.tb_Product.Find(id);
            if (item != null)
            {
                item.IsHot = !item.IsHot;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, IsHot = item.IsHot });
            }

            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult IsFeature(int id)
        {
            var item = db.tb_Product.Find(id);
            if (item != null)
            {
                item.IsFeature = !item.IsFeature;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, IsFeature = item.IsFeature });
            }

            return Json(new { success = false });
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
