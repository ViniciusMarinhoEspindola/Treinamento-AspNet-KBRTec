namespace SistemaDeAtendimento.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConsultorMigration : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Order");
            AddColumn("dbo.AspNetUsers", "OrdemRegistros", c => c.Int(nullable: false, identity: true));
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Order", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.AspNetUsers", "OrdemRegistros");
        }
    }
}
