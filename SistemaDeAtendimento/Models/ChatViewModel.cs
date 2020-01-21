using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaDeAtendimento.Models
{
    public class ChatViewModel
    {
        [DataType(DataType.Upload)]
        [Display(Name = "Arquivo de Upload")]
        public HttpPostedFileBase Arq { get; set; }
    }
}