using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SistemaDeAtendimento.Entity;
using System.Net.Mail;
using SistemaDeAtendimento.Areas.Admin.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using SistemaDeAtendimento.Models;

namespace SistemaDeAtendimento.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ConsultoresController : Controller
    {   
        private SistemaAtendimentoEntities db = new SistemaAtendimentoEntities();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ConsultoresController()
        {
        }

        public ConsultoresController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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


        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.Message = TempData["Message"];
            var User = db.AspNetRoles.Where(s => s.Name == "Consultor").FirstOrDefault().AspNetUsers.OrderByDescending(s => s.OrdemRegistros).ToList();
            return View(User);
        }
        
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]

        [ValidateAntiForgeryToken]
        
        public async Task<ActionResult> Register(Models.RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

                var file = model.Foto;
                string foto = null;
                if (file != null)
                {
                    foto = Guid.NewGuid().ToString() + System.IO.Path.GetFileName(file.FileName);
                    string path = System.IO.Path.Combine(Server.MapPath("~/images"), foto);
                    file.SaveAs(path);
                }
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Nome = model.Nome,
                    Foto = foto,
                    Descricao = model.Descricao,
                    Status = "Ocupado"
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    await UserManager.SendEmailAsync(user.Id, "Aviso de cadastro", "Você foi cadastrado como consultor no sistema de atendimento com as seguintes informações: \n E-mail: " + user.Email + "\n Senha: " + model.Password);
                    //Email ^

                    // Para obter mais informações sobre como habilitar a confirmação da conta e redefinição de senha, visite https://go.microsoft.com/fwlink/?LinkID=320771
                    // Enviar um email com este link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirmar sua conta", "Confirme sua conta clicando <a href=\"" + callbackUrl + "\">aqui</a>");

                    UserManager.AddToRole(user.Id, "Consultor");

                    TempData["Message"] = "Cadastrado com sucesso!";
                    return RedirectToAction("Index");
                }
                AddErrors(result);
            }

            // Se chegamos até aqui e houver alguma falha, exiba novamente o formulário
            return View(model);
        }




        // GET: Admin/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            if (aspNetUsers == null)
            {
                return HttpNotFound();
            }
            var model = new EditViewModel
            {
                Id = aspNetUsers.Id,
                Nome = aspNetUsers.Nome,
                Email = aspNetUsers.Email,
                FotoAntiga = aspNetUsers.Foto,
                Descricao = aspNetUsers.Descricao
            };
            return View(model);
        }

        // POST: Admin/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditViewModel editViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = db.AspNetUsers.Find(editViewModel.Id);
                var file = editViewModel.Foto;
                string foto = null;
                if (file != null)
                {
                    if (editViewModel.FotoAntiga != null)
                        System.IO.File.Delete(System.IO.Path.Combine(Server.MapPath("~/images"), editViewModel.FotoAntiga));

                    foto = Guid.NewGuid().ToString() + System.IO.Path.GetFileName(file.FileName);
                    string path = System.IO.Path.Combine(Server.MapPath("~/images"), foto);
                    file.SaveAs(path);
                    user.Foto = foto;
                }
                if (!user.Email.Equals(editViewModel.Email))
                {
                    user.Email = editViewModel.Email;
                    user.UserName = editViewModel.Email;
                }
                user.Nome = editViewModel.Nome;
                user.Descricao = editViewModel.Descricao;

                
                db.SaveChanges();
                TempData["Message"] = "Editado com sucesso!";
                return RedirectToAction("Index");
            }
            return View(editViewModel);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            if (aspNetUsers == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUsers);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            System.IO.File.Delete(System.IO.Path.Combine(Server.MapPath("~/images"), aspNetUsers.Foto));
            db.AspNetUsers.Remove(aspNetUsers);
            db.SaveChanges();

            TempData["Message"] = "Deletado com sucesso!";
            return RedirectToAction("Index");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}