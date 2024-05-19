using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DoAn2024_NguyenTuanAnh_202060621_Demo
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
            name: "ShoppingCart",
            url: "gio-hang",
            defaults: new { controller = "ShoppingCart", action = "Index", alias = UrlParameter.Optional },
            namespaces: new[] { "DoAn2024_NguyenTuanAnh_202060621_Demo.Controllers" }
        );
            routes.MapRoute(
            name: "InformationUser",
            url: "thong-tin",
            defaults: new { controller = "Home", action = "InformationUser", alias = UrlParameter.Optional },
            namespaces: new[] { "DoAn2024_NguyenTuanAnh_202060621_Demo.Controllers" }
        );
            routes.MapRoute(
            name: "Contact",
            url: "lien-he",
            defaults: new { controller = "Contact", action = "Index", alias = UrlParameter.Optional },
            namespaces: new[] { "DoAn2024_NguyenTuanAnh_202060621_Demo.Controllers" }
        );
            routes.MapRoute(
           name: "Introduce",
           url: "gioi-thieu",
           defaults: new { controller = "Home", action = "Introduce", alias = UrlParameter.Optional },
           namespaces: new[] { "DoAn2024_NguyenTuanAnh_202060621_Demo.Controllers" }
       );

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
            name: "CategoryProduct",
            url: "san-pham/{alias}-{id}",
            defaults: new { controller = "Products", action = "ProductCategory", alias = UrlParameter.Optional },
            namespaces: new[] { "DoAn2024_NguyenTuanAnh_202060621_Demo.Controllers" }
        );
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
            name: "Products",
            url: "san-pham",
            defaults: new { controller = "Products", action = "HienThi", id = UrlParameter.Optional },
            namespaces: new[] { "DoAn2024_NguyenTuanAnh_202060621_Demo.Controllers" }
        );
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
            name: "HCN",
            url: "san-pham-hcn",
            defaults: new { controller = "Products", action = "HCN", id = UrlParameter.Optional },
            namespaces: new[] { "DoAn2024_NguyenTuanAnh_202060621_Demo.Controllers" }
        );
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
            name: "Oval",
            url: "san-pham-oval",
            defaults: new { controller = "Products", action = "Oval", id = UrlParameter.Optional },
            namespaces: new[] { "DoAn2024_NguyenTuanAnh_202060621_Demo.Controllers" }
        );
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
            name: "Tron",
            url: "san-pham-tron",
            defaults: new { controller = "Products", action = "Tron", id = UrlParameter.Optional },
            namespaces: new[] { "DoAn2024_NguyenTuanAnh_202060621_Demo.Controllers" }
        );
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
            name: "Price1",
            url: "san-pham-1",
            defaults: new { controller = "Products", action = "Price1", id = UrlParameter.Optional },
            namespaces: new[] { "DoAn2024_NguyenTuanAnh_202060621_Demo.Controllers" }
        );
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
            name: "Price2",
            url: "san-pham-2",
            defaults: new { controller = "Products", action = "Price2", id = UrlParameter.Optional },
            namespaces: new[] { "DoAn2024_NguyenTuanAnh_202060621_Demo.Controllers" }
        );
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
            name: "Price3",
            url: "san-pham-3",
            defaults: new { controller = "Products", action = "Price3", id = UrlParameter.Optional },
            namespaces: new[] { "DoAn2024_NguyenTuanAnh_202060621_Demo.Controllers" }
        );
            routes.MapRoute(
        name: "CheckOut",
        url: "thanh-toan",
        defaults: new { controller = "ShoppingCart", action = "CheckOut", alias = UrlParameter.Optional },
        namespaces: new[] { "DoAn2024_NguyenTuanAnh_202060621_Demo.Controllers" }
         );
            routes.MapRoute(
            name: "chitietProduct",
            url: "chi-tiet/{alias}-{id}",
            defaults: new { controller = "Products", action = "ChiTiet", alias = UrlParameter.Optional },
            namespaces: new[] { "DoAn2024_NguyenTuanAnh_202060621_Demo.Controllers" }
        );
            routes.MapRoute(
         name: "DetailNew",
         url: "tin-tuc/{alias}-{id}",
         defaults: new { controller = "News", action = "Detail", id = UrlParameter.Optional },
         namespaces: new[] { "DoAn2024_NguyenTuanAnh_202060621_Demo.Controllers" }
     );
            routes.MapRoute(
           name: "NewsList",
           url: "tin-tuc",
           defaults: new { controller = "News", action = "Index", alias = UrlParameter.Optional },
           namespaces: new[] { "DoAn2024_NguyenTuanAnh_202060621_Demo.Controllers" }
       );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
              namespaces: new[] { "DoAn2024_NguyenTuanAnh_202060621_Demo.Controllers" }
            );


        }
    }
}
