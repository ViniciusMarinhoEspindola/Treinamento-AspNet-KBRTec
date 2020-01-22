using SistemaDeAtendimento.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaDeAtendimento.Controllers
{
    public class HomeController : Controller
    {
        private SistemaAtendimentoEntities db = new SistemaAtendimentoEntities();
        public ActionResult Index()
        {
            var User = db.AspNetRoles.Where(s => s.Name == "Consultor").FirstOrDefault().AspNetUsers.Where(a => a.Status == "Disponível").OrderByDescending(s => s.OrdemRegistros).ToList();
            ViewBag.Message = TempData["Message"];
            return View(User);
        }

        public ActionResult Visitante(string idConsultor)
        {
            
            var user = db.AspNetUsers.Find(idConsultor);
            if (user.Status == "Ocupado")
            {
                TempData["Message"] = "Desculpe, Mas parece que este consultor acabou de ficar ocupado! Tente novamente com outro consultor.";
                return RedirectToAction("index", "Home");
            }
            ViewBag.idConsultor = idConsultor;
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Visitante(string groupId, [Bind(Include = "Nome,Email,Celular,Duracao")] Visitante visitante)
        {
            if (ModelState.IsValid)
            {
                var visita = db.Visitante.Add(visitante);
                db.SaveChanges();
                
                return RedirectToAction("Entrar", "Chat", new { groupId = groupId, visitanteId = visita.IdVisitante });
            }
            return View(visitante);
        }
    }
}