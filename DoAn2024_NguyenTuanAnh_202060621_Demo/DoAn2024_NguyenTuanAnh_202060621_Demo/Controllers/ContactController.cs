using DoAn2024_NguyenTuanAnh_202060621_Demo.Models.AutoAdd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn2024_NguyenTuanAnh_202060621_Demo.Controllers
{
    public class ContactController : Controller
    {
        private WenBanHangOnline db = new WenBanHangOnline();
        // GET: Contact
        public ActionResult Index()
        {
            var item = new tb_Contact();
            var nhanVien = (Models.AutoAdd.User)Session["user"];
            if (nhanVien != null)
            {
                var user = db.Users.SingleOrDefault(m => m.UserName == nhanVien.UserName);
                if (user != null)
                {
                    item.Email = user.Email;
                    item.Name = user.Name;
                    item.UserName = user.UserName;
                }
                return View(item);
            }
            
            return View();
        }
        [HttpPost]
        public ActionResult PostContact(tb_Contact req)
        {


            if (ModelState.IsValid)
            {
                req.CreatedDate = DateTime.Now;
                db.tb_Contact.Add(req);
                db.SaveChanges();
                return Json(new { Success = true });
            }

            return Json(new { Success = false });
        }
    }
}