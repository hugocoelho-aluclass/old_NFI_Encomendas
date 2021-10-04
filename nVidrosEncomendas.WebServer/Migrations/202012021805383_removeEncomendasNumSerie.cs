namespace NVidrosEncomendas.WebServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeEncomendasNumSerie : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Encomendas", "NumSerieEncomenda");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Encomendas", "NumSerieEncomenda", c => c.String());
        }
    }
}
