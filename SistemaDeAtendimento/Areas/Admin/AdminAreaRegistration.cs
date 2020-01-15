using System.Web.Mvc;

namespace SistemaDeAtendimento.Areas.Admin
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
                new { controller = "Consultores", action = "Index", id = UrlParameter.Optional },
                new[] { "SistemaDeAtendimento.Areas.Admin.Controllers" }
            );
        }
    }
}