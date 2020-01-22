using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaDeAtendimento.Models
{
    public class ChatViewModel
    {
        [Display(Name = "Tempo")]
        public int tempo { get; set; }
    }
}