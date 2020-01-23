using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SistemaDeAtendimento.Areas.Consultores.Models;
using SistemaDeAtendimento.Entity;

namespace SistemaDeAtendimento.Areas.Consultores.Controllers
{
    [Authorize(Roles = "Consultor")]
    public class ConsultorController : Controller
    {
        private SistemaAtendimentoEntities db = new SistemaAtendimentoEntities();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ConsultorController()
        {
        }

        public ConsultorController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Consultores/Consultor
        public ActionResult Index()
        {
            var consultor = User.Identity.GetUserId();
            var Atendimentos = db.Conversa.Where(s => s.ConsultorId == consultor.ToString()).Where(s => s.VisitanteId != null).OrderByDescending(s => s.IdConversa).ToList();
            ViewBag.Message = TempData["Message"];
            //ViewBag.Status = Atendimentos.First().AspNetUsers.Status;
            return View(Atendimentos);
        }

        public ActionResult Mensagens(int conversa)
        {
            var mensagens = db.Mensagens.Where(s => s.ConversaId == conversa);

            return View(mensagens);
        }

        // GET: /Admin/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Admin/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordConsultorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    TempData["Message"] = "Senha alterada com sucesso!";
                }

                return RedirectToAction("Index");
            }
            AddErrors(result);
            return View(model);
        }

        public ActionResult ChangeStatus(string UserId)
        {
            var user = db.AspNetUsers.Find(UserId);
            var TrocaStatus = user.Status == "Ocupado" ? user.Status = "Disponível" : user.Status = "Ocupado";
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}