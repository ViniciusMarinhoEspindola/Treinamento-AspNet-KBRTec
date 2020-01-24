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
            
            ViewBag.Message = TempData["Message"];
            return View(User);
        }

        public ActionResult Visitante(string Id)
        {
            
            var user = db.AspNetUsers.Find(Id);
            if (user.Status == "Ocupado")
            {
                TempData["Message"] = "Desculpe, Mas parece que este consultor acabou de ficar ocupado! Tente novamente com outro consultor.";
                return RedirectToAction("index", "Home");
            }
            ViewBag.idConsultor = Id;
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Visitante(string Id, [Bind(Include = "Nome,Email,Celular,Duracao")] Visitante visitante)
        {
            if (ModelState.IsValid)
            {
                var visita = db.Visitante.Add(visitante);
                db.SaveChanges();
                
                return RedirectToAction("Entrar", "Chat", new { groupId = Id, visitanteId = visita.IdVisitante });
            }
            return View(visitante);
        }
    }
}