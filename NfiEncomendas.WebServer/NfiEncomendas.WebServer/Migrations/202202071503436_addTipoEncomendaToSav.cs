namespace NfiEncomendas.WebServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTipoEncomendaToSav : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Savs", "TipoEncomenda_IdTipoEncomenda", c => c.Int());
            CreateIndex("dbo.Savs", "TipoEncomenda_IdTipoEncomenda");
            AddForeignKey("dbo.Savs", "TipoEncomenda_IdTipoEncomenda", "dbo.TipoEncomendas", "IdTipoEncomenda");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Savs", "TipoEncomenda_IdTipoEncomenda", "dbo.TipoEncomendas");
            DropIndex("dbo.Savs", new[] { "TipoEncomenda_IdTipoEncomenda" });
            DropColumn("dbo.Savs", "TipoEncomenda_IdTipoEncomenda");
        }
    }
}
