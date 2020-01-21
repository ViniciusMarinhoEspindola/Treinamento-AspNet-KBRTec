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
                ViewBag.Consultor = 1;
                ViewBag.GroupId = User.Identity.GetUserId();
                ViewBag.Nome = User.Identity.GetName();
            }
            else
            {
                ViewBag.Consultor = 0;
                ViewBag.GroupId = TempData["groupId"];
                var visitante = db.Visitante.Find(TempData["visitanteId"]);
                ViewBag.Nome = visitante.Nome;
            }
            return View();
        }

        public ActionResult Upload(HttpPostedFileBase arq, string pathUpl)
        {
            var file = arq;
            pathUpl = "Conversa" + pathUpl;
            string upl = null;
            if (file != null)
            {
                upl = System.IO.Path.GetFileName(file.FileName);
                System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath("~/upload/") + pathUpl);
                string path = System.IO.Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/upload/" + pathUpl), upl);
                file.SaveAs(path);
            }

            return Json(upl);
        }

        public void Download(string path, string filename)
        {

            if (filename != "")
            {
                path = Server.MapPath("/upload/Conversa" + path + "/" + filename);
                System.IO.FileInfo file = new System.IO.FileInfo(path);

                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.WriteFile(file.FullName);
                Response.End();
            }
        }

        public ActionResult Entrar(string groupId, int? visitanteId)
        {
            //
            var verificaConsultor = db.Conversa.Where(s => s.ConsultorId == groupId).Where(a => a.VisitanteId == null).Count();
            var IdConversa = 0;

            TempData["groupId"] = groupId;
           
            if (verificaConsultor.Equals(0))
            {
                var conversa = new Conversa { VisitanteId = visitanteId, ConsultorId = groupId, dtConversa = DateTime.Now };
                db.Conversa.Add(conversa);
                db.SaveChanges();
                IdConversa = conversa.IdConversa;


            } else
            {
                var conversa = db.Conversa.Where(s => s.ConsultorId == groupId).Where(s => s.VisitanteId == null).OrderByDescending(a => a.IdConversa).First();
                var torcaStatus = db.AspNetUsers.Find(groupId);

                if (User.IsInRole("Consultor"))
                {
                    TempData["visitanteId"] = 0;                    
                    torcaStatus.Status = "Disponível";
                    db.SaveChanges();
                    return RedirectToAction("index", "Chat", new { IdConversa = conversa.IdConversa });
                }

                conversa.VisitanteId = visitanteId;
                IdConversa = conversa.IdConversa;

                torcaStatus.Status = "Ocupado";
                
            }
            TempData["visitanteId"] = visitanteId;
            if (!User.IsInRole("Consultor"))
            {
                //Cadastrar nova Notificação banco
                //var visitante = db.Visitante.Find(visitanteId);
                //var mensagemNotificacao = "O usuário " + visitante.Nome + " solicitou uma conversa no chat.";
                //var notificacao = new Notificacoes { ConsultorId = groupId, ConversaId = visitanteId, MensagemNotificacao = mensagemNotificacao };
                //db.Notificacoes.Add(notificacao);
            }
            db.SaveChanges();
            
            return RedirectToAction("index", "Chat", new { IdConversa = IdConversa });
        }
    }
}