namespace NVidrosEncomendas.WebServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeArtigoToRefObra : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Encomendas", "RefObra", c => c.String());
            DropColumn("dbo.Encomendas", "NomeArtigo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Encomendas", "NomeArtigo", c => c.String());
            DropColumn("dbo.Encomendas", "RefObra");
        }
    }
}
