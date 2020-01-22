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
        public int? Duracao { get; set; }

        [Display(Name = "Conversa")]
        public int? IdConversa { get; set; }

        [Display(Name = "Visitante")]
        public int? IdVisitante { get; set; }

        [Display(Name = "Consultor")]
        public string consultor { get; set; }

        [Display(Name = "Nome do consultor")]
        public string nm_consultor { get; set; }

        [Display(Name = "Nome do cliente")]
        public string nm_visitante { get; set; }
    }
}