namespace NfiEncomendas.WebServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAnoEDataProduzido : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Encomendas", "AnoEntrega", c => c.Int());
            AddColumn("dbo.Encomendas", "DataProduzido", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Encomendas", "DataProduzido");
            DropColumn("dbo.Encomendas", "AnoEntrega");
        }
    }
}
