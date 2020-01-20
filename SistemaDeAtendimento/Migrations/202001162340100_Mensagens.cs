namespace SistemaDeAtendimento.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mensagens : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Visitante",
                c => new
                {
                    IdVisitante = c.Int(nullable: false, identity: true),
                    Nome = c.String(),
                    Email = c.String(),
                    Celular = c.String(),
                })
                .PrimaryKey(t => t.IdVisitante);

            CreateTable(
                "dbo.Conversa",
                c => new
                {
                    IdConversa = c.Int(nullable: false, identity: true),
                    ConsultorId = c.String(nullable: false, maxLength: 128),
                    VisitanteId = c.Int(nullable: true),
                    dataConversa = c.DateTimeOffset(),
                })
                .PrimaryKey(t => new { t.IdConversa });

            CreateTable(
                "dbo.Mensagens",
                c => new
                {
                    IdMensagem = c.Int(nullable: false, identity: true),
                    Mensagem = c.String(),
                    Remetente = c.Boolean(),
                    data = c.DateTime(),
                    ConversaId = c.Int(),
                })
                .PrimaryKey(t => new { t.IdMensagem });
                
        }
        
        public override void Down()
        {
        }
    }
}
