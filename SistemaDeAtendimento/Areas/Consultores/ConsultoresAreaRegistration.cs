using System.Web.Mvc;

namespace SistemaDeAtendimento.Areas.Consultores
{
    public class ConsultoresAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Consultores";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Consultores_default",
                "Consultores/{controller}/{action}/{id}",
                new { controller = "Consultor", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}