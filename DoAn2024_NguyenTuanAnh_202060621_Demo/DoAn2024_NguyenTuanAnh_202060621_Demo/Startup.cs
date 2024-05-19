using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DoAn2024_NguyenTuanAnh_202060621_Demo.Startup))]
namespace DoAn2024_NguyenTuanAnh_202060621_Demo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
