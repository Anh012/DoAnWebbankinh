using System.Web.Mvc;

namespace DoAn2024_NguyenTuanAnh_202060621_Demo.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                 namespaces: new[] { "DoAn2024_NguyenTuanAnh_202060621_Demo.Areas.Admin.Controllers" }
            );
        }
    }
}