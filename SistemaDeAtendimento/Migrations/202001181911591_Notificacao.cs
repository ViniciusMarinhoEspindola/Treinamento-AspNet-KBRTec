namespace SistemaDeAtendimento.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Notificacao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notificacoes",
                c => new
                {
                    IdNotificacao = c.Int(nullable: false, identity: true),
                    MensagemNotificacao = c.String(),
                    ConversaId = c.Int(),
                    ConsultorId = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => new { t.IdNotificacao });
        }
        
        public override void Down()
        {
        }
    }
}
