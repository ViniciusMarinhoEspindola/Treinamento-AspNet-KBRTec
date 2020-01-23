using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaDeAtendimento.ChatHub;
using SistemaDeAtendimento.Entity;
using Microsoft.AspNet.Identity;
using SistemaDeAtendimento.App_Start;
using SistemaDeAtendimento.Models;
<<<<<<< HEAD
using SistemaDeAtendimento.Helpers;
=======
>>>>>>> 072176b4d1ee1f44a6916aefd9a095c25fa1abb6

namespace SistemaDeAtendimento.Controllers
{
    public class ChatController : Controller
    {
        private SistemaAtendimentoEntities db = new SistemaAtendimentoEntities();
        private List<ChatViewModel> modelChat = new List<ChatViewModel>();
        // GET: Chat
        public ActionResult Index(int Id)
        {
            var conversa = db.Conversa.Find(Id);

<<<<<<< HEAD
            //ViewBag.Tempo = int.Parse(Cookies.Get("Tempo"));

            if (User.IsInRole("Consultor"))
            {
                ViewBag.Consultor = 1;
                ViewBag.Cronometro = 0;
            }
            else
            {
                ViewBag.Cronometro = TempData["cronometro"];
                if (ViewBag.Cronometro != 0)
                {
                    TempData["Message"] = "Desculpe, mas você se desconectou do chat!";
                    return RedirectToAction("index", "Home");
                }
                ViewBag.Consultor = 0;
            }
            //var variavel = modelChat.FirstOrDefault(x => x.IdVisitante == conversa.VisitanteId);
            //variavel.IdConversa = conversa.IdConversa;
            //variavel.consultor = conversa.AspNetUsers.Id;
            //variavel.nm_consultor = conversa.AspNetUsers.Nome;
            //variavel.nm_visitante = conversa.Visitante.Nome;

            return View(conversa);
=======
            if (User.IsInRole("Consultor"))
            {
                ViewBag.Consultor = 1;
                //ViewBag.Cronometro = 0;
            }
            else
            {
                //ViewBag.Cronometro = TempData["cronometro"];
                //if (ViewBag.Cronometro != 0)
                //{
                //    TempData["Message"] = "Desculpe, mas você se desconectou do chat!";
                //    return RedirectToAction("index", "Home");
                //}
                ViewBag.Consultor = 0;
            }
            var variavel = modelChat.FirstOrDefault(x => x.IdVisitante == conversa.VisitanteId);
            variavel.IdConversa = conversa.IdConversa;
            variavel.consultor = conversa.AspNetUsers.Id;
            variavel.nm_consultor = conversa.AspNetUsers.Nome;
            variavel.nm_visitante = conversa.Visitante.Nome;

            return View(variavel);
>>>>>>> 072176b4d1ee1f44a6916aefd9a095c25fa1abb6
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
            var visitante = db.Visitante.Find(visitanteId);
            //Cookies.Set("Tempo", (visitante.Duracao * 60).ToString());
            TempData["cronometro"] = 0;
            var verificaConsultor = db.Conversa.Where(s => s.ConsultorId == groupId).Where(a => a.VisitanteId == null).Count();
            var IdConversa = 0;
           
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
                    torcaStatus.Status = "Ocupado";
                    db.SaveChanges();
                    return RedirectToAction("index", "Chat", new { Id = conversa.IdConversa });
                }

                conversa.VisitanteId = visitanteId;
                IdConversa = conversa.IdConversa;

                torcaStatus.Status = "Ocupado";
                
            }
<<<<<<< HEAD
            //var visitante = db.Visitante.Find(visitanteId);
            //modelChat.Add(new ChatViewModel { Duracao = (visitante.Duracao * 60), IdVisitante = visitante.IdVisitante });
=======
            var visitante = db.Visitante.Find(visitanteId);
            modelChat.Add(new ChatViewModel { Duracao = (visitante.Duracao * 60), IdVisitante = visitante.IdVisitante });
>>>>>>> 072176b4d1ee1f44a6916aefd9a095c25fa1abb6

            db.SaveChanges();
            

            return RedirectToAction("index", "Chat", new { Id = IdConversa });
        }
    }
}