namespace NVidrosEncomendas.WebServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDataExpedido1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Encomendas", "DataExpedido", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Encomendas", "DataExpedido", c => c.DateTime());
        }
    }
}
