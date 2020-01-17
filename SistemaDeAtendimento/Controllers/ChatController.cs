using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaDeAtendimento.ChatHub;
using SistemaDeAtendimento.Entity;
using Microsoft.AspNet.Identity;
using SistemaDeAtendimento.App_Start;

namespace SistemaDeAtendimento.Controllers
{
    public class ChatController : Controller
    {
        private SistemaAtendimentoEntities db = new SistemaAtendimentoEntities();
        // GET: Chat
        public ActionResult Index(int IdConversa)
        {
            ViewBag.IdConversa = IdConversa;
            if (User.IsInRole("Consultor"))
            {
                ViewBag.Consultor = true;
                ViewBag.GroupId = User.Identity.GetUserId();
                ViewBag.Nome = User.Identity.GetName();
            }
            else
            {
                //if (!string.IsNullOrEmpty(TempData["groupId"].ToString()))
                //{
                //    var user = db.AspNetUsers.Find(TempData["groupId"]);
                //    if (user.Status == "Ocupado")
                //        return RedirectToAction("index", "Home");
                //}
                ViewBag.Consultor = false;
                ViewBag.GroupId = TempData["groupId"];
                var visitante = db.Visitante.Find(TempData["visitanteId"]);
                ViewBag.Nome = visitante.Nome;
            }
            //var User = db.AspNetUsers.Where(s => s.Id == TempData["groupId"]).First();
            return View();
        }

        public ActionResult Entrar(string groupId, int visitanteId = 0)
        {
            //
            var verificaConsultor = db.Conversa.Where(s => s.ConsultorId == groupId).Where(a => a.VisitanteId == 0).Count();
            
            if (verificaConsultor.Equals(0))
            {
                var conversa = new Conversa { VisitanteId = visitanteId, ConsultorId = groupId };
                db.Conversa.Add(conversa);
                db.SaveChanges();
                //Notificação
            }

            if (visitanteId != 0)
            {    
                if(!verificaConsultor.Equals(0)) 
                {
                    var conversa = db.Conversa.Where(s => s.ConsultorId == groupId).Where(s => s.VisitanteId == 0).OrderByDescending(a => a.IdConversa).First();
                    conversa.VisitanteId = visitanteId;
                    db.SaveChanges();
                }
                    
                var torcaStatus = db.AspNetUsers.Find(groupId);
                torcaStatus.Status = "Ocupado";
                db.SaveChanges();
                TempData["visitanteId"] = visitanteId;
            } else
            {
                if (!verificaConsultor.Equals(0))
                {
                    var conversa = db.Conversa.Where(s => s.ConsultorId == groupId).Where(s => s.VisitanteId == 0).OrderByDescending(a => a.IdConversa).First();
                    return RedirectToAction("index", "Chat", new { IdConversa = conversa.IdConversa });
                }
                TempData["visitanteId"] = "Sem visitante";
            }

            TempData["groupId"] = groupId;
            
            return RedirectToAction("index", "Chat", new { IdConversa = conversa.IdConversa });
        }
    }
}