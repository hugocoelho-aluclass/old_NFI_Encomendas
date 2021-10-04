namespace NVidrosEncomendas.WebServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class upd : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Encomendas", "TipoEncomenda_IdTipoEncomenda", "dbo.TipoEncomendas");
            DropIndex("dbo.Encomendas", new[] { "TipoEncomenda_IdTipoEncomenda" });
            DropColumn("dbo.Encomendas", "TipoEncomenda_IdTipoEncomenda");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Encomendas", "TipoEncomenda_IdTipoEncomenda", c => c.Int());
            CreateIndex("dbo.Encomendas", "TipoEncomenda_IdTipoEncomenda");
            AddForeignKey("dbo.Encomendas", "TipoEncomenda_IdTipoEncomenda", "dbo.TipoEncomendas", "IdTipoEncomenda");
        }
    }
}
