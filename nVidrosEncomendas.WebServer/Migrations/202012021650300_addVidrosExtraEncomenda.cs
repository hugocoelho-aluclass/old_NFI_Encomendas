namespace NVidrosEncomendas.WebServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addVidrosExtraEncomenda : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Encomendas", "NomeArtigo", c => c.String());
            AddColumn("dbo.Encomendas", "VidrosExtra_Id", c => c.Int());
            CreateIndex("dbo.Encomendas", "VidrosExtra_Id");
            AddForeignKey("dbo.Encomendas", "VidrosExtra_Id", "dbo.VidrosExtras", "Id");
            DropColumn("dbo.Encomendas", "NomeObra");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Encomendas", "NomeObra", c => c.String());
            DropForeignKey("dbo.Encomendas", "VidrosExtra_Id", "dbo.VidrosExtras");
            DropIndex("dbo.Encomendas", new[] { "VidrosExtra_Id" });
            DropColumn("dbo.Encomendas", "VidrosExtra_Id");
            DropColumn("dbo.Encomendas", "NomeArtigo");
        }
    }
}
