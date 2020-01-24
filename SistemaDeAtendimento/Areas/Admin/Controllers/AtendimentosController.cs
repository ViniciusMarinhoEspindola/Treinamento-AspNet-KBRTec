using SistemaDeAtendimento.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaDeAtendimento.Areas.Admin.Controllers
{
    public class AtendimentosController : Controller
    {
        private SistemaAtendimentoEntities db = new SistemaAtendimentoEntities();
        // GET: Admin/Atendimentos
        public ActionResult Index(string Id)
        {
            var Atendimentos = db.Conversa.Where(s => s.ConsultorId == Id).Where(s => s.VisitanteId != null).OrderByDescending(s => s.IdConversa).ToList();
            return View(Atendimentos);
        }

        public ActionResult Mensagens(int Id)
        {
            var msg = db.Mensagens.Where(s => s.ConversaId == Id).Count();
            if (msg > 0)
            {
                var mensagens = db.Mensagens.Where(s => s.ConversaId == Id);
                return View(mensagens);
            }
            else
            {
                return RedirectToAction("Mensagens404");
            }

        }

        public ActionResult Mensagens404()
        {
            return View();
        }
    }
}