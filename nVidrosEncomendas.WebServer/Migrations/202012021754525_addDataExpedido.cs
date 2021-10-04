namespace NVidrosEncomendas.WebServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDataExpedido : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Encomendas", "DataExpedido", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Encomendas", "DataExpedido");
        }
    }
}
