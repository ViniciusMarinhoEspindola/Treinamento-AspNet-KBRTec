//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SistemaDeAtendimento.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class Mensagens
    {
        public int IdMensagem { get; set; }
        public string Mensagem { get; set; }
        public Nullable<bool> Remetente { get; set; }
        public System.DateTime data { get; set; }
        public int ConversaId { get; set; }
    
        public virtual Conversa Conversa { get; set; }
    }
}
