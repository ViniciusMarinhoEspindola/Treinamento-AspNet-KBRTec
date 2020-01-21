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
        public ActionResult Index(string consultor)
        {
            var Atendimentos = db.Conversa.Where(s => s.ConsultorId == consultor).Where(s => s.VisitanteId != null).OrderByDescending(s => s.IdConversa).ToList();
            return View(Atendimentos);
        }

        public ActionResult Mensagens(int conversa)
        {
            var mensagens = db.Mensagens.Where(s => s.ConversaId == conversa);

            return View(mensagens);
        }
    }
}