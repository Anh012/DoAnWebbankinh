using DoAn2024_NguyenTuanAnh_202060621_Demo.Models.AutoAdd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DoAn2024_NguyenTuanAnh_202060621_Demo.App_Start
{
    public class AdminAuthorize : AuthorizeAttribute
    {
        public string IdChucNang { get; set; }

        //1check dang nhap
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            User nvSession = (User)HttpContext.Current.Session["user"];
            if (nvSession != null)
            {
                //2 check quyen: true => thuc hien filter 
                //nguoc lai tro lai trang Error
                WenBanHangOnline dbb = new WenBanHangOnline();

                var roles = IdChucNang.Split(',').Select(int.Parse);
                var count = dbb.UserRoles.Count(m => m.idUser == nvSession.Id && roles.Contains(m.idRole));
                if (count != 0)
                {
                    
                        return;
                    
                   
                }
                else
                {                                      
                    var returnurl = filterContext.RequestContext.HttpContext.Request.RawUrl;
                    filterContext.Result = new RedirectToRouteResult(new
                      RouteValueDictionary(new { Controller = "Error", action = "KhongCoQuyen", area = "Admin", returnurl = returnurl.ToString() }));

                }
                return;
            }
            else
            {
                var returnurl = filterContext.RequestContext.HttpContext.Request.RawUrl;
                filterContext.Result = new RedirectToRouteResult(new
                  RouteValueDictionary(new { Controller = "Home", action = "Index", area = "Admin", returnurl = returnurl.ToString() }));
            }
        }
    }
    //public class AdminAuthorize : AuthorizeAttribute
    //{
    //    public int idChucNang {  get; set; }
    //    //1check dang nhap
    //    public override void OnAuthorization(AuthorizationContext filterContext)
    //    {
    //        User nvSession = (User)HttpContext.Current.Session["user"];
    //        if (nvSession != null)
    //        {
    //            //2 check quyen: true => thuc hien filter 
    //            //nguoc lai tro lai trang Error
    //            DbUser dbb = new DbUser();

    //            var count = dbb.UserRoles.Count(m => m.idUser == nvSession.Id && m.idRole == idChucNang);
    //            if (count != 0)
    //            {
    //                return;
    //            }
    //            else
    //            {
    //                var returnurl = filterContext.RequestContext.HttpContext.Request.RawUrl;
    //                filterContext.Result = new RedirectToRouteResult(new
    //                  RouteValueDictionary(new { Controller = "Error", action = "KhongCoQuyen", area = "Admin", returnurl = returnurl.ToString() }));

    //            }
    //            return;
    //        }else 
    //        {
    //            var returnurl = filterContext.RequestContext.HttpContext.Request.RawUrl;
    //            filterContext.Result = new RedirectToRouteResult(new
    //              RouteValueDictionary(new { Controller = "Home", action = "Index", area = "Admin", returnurl = returnurl.ToString() }));
    //        }


    //    }
    //}
}