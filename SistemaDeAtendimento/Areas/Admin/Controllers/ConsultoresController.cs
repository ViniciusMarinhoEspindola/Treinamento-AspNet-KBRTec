using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaDeAtendimento.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ConsultoresController : Controller
    {
        // GET: Admin/Consultores
        public ActionResult Index()
        {
            return View();
        }
    }
}