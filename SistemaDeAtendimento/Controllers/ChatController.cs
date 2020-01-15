using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaDeAtendimento.ChatHub;
using SistemaDeAtendimento.Entity;
using Microsoft.AspNet.Identity;

namespace SistemaDeAtendimento.Controllers
{
    public class ChatController : Controller
    {
        private SistemaAtendimentoEntities db = new SistemaAtendimentoEntities();
        // GET: Chat
        public ActionResult Index()
        {
            if (User.IsInRole("Consultor"))
            {
                ViewBag.GroupId = User.Identity.GetUserId();
            }
            else
            {
                ViewBag.GroupId = TempData["groupId"];
            }
            //var User = db.AspNetUsers.Where(s => s.Id == TempData["groupId"]).First();
            return View();
        }

        public ActionResult Entrar(string groupId)
        {
            var chat = new ChatHub.ChatHub();
            chat.AddToGroup(groupId);
            TempData["groupId"] = groupId;
            return RedirectToAction("index");
        }
    }
}