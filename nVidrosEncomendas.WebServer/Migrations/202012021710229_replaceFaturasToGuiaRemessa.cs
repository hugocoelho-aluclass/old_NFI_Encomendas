namespace NVidrosEncomendas.WebServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class replaceFaturasToGuiaRemessa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Encomendas", "GuiaRemessa", c => c.String());
            DropColumn("dbo.Encomendas", "Fatura");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Encomendas", "Fatura", c => c.String());
            DropColumn("dbo.Encomendas", "GuiaRemessa");
        }
    }
}
