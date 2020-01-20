using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaDeAtendimento.Areas.Consultores.Models
{
    public class ConsultorViewModel
    {
        [Required]
        [Display(Name = "Id")]
        public int IdConversa { get; set; }

        [Required]
        [Display(Name = "Consultor")]
        public string consultor { get; set; }

        [Required]
        [Display(Name = "Cliente")]
        public string cliente { get; set; }

        [Required]
        [Display(Name = "Data")]
        public DateTimeOffset date { get; set; }
    }
    public class ChangePasswordConsultorViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha atual")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} deve ter pelo menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nova senha")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar nova senha")]
        [Compare("NewPassword", ErrorMessage = "A nova senha e a senha de confirmação não correspondem.")]
        public string ConfirmPassword { get; set; }
    }
}