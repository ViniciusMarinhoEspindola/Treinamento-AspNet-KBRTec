namespace SistemaDeAtendimento.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Files : DbMigration
    {
        public override void Up()
        {
            AddColumn(
                "dbo.Visitante",
                "Duracao", c => c.Int()
            );

            AddColumn(
                "dbo.Mensagens",
                "Arquivos", c => c.String()
            );
        }
        
        public override void Down()
        {
            DropColumn("dbo.Conversa", "duracao");
            DropColumn("dbo.Mensagens", "Arquivos");
        }
    }
}
