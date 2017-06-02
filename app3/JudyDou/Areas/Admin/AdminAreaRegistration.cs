using System.Web.Mvc;

namespace JudyDou.Areas.Admin
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
                "Admin_home",
                "Admin/",
                new { controller = "Listing", action = "Index" },
                new[] { "JudyDou.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "JudyDou.Areas.Admin.Controllers" }
            );
        }
    }
}
