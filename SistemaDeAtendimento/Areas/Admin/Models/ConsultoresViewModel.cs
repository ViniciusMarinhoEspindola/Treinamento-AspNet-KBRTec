using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaDeAtendimento.Areas.Admin.Models
{
    public class EditViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Foto de Perfil")]
        public HttpPostedFileBase Foto { get; set; }
        public string FotoAntiga { get; set; }
        public string Descricao { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        public string Nome { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Foto de Perfil")]
        public HttpPostedFileBase Foto { get; set; }

        [Required]
        public string Descricao { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "O/A {0} deve ter no mínimo {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Senha")]
        [Compare("Password", ErrorMessage = "A senha e a senha de confirmação não correspondem.")]
        public string ConfirmPassword { get; set; }
    }
}