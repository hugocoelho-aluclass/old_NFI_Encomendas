namespace NVidrosEncomendas.WebServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTiposEncomendaEncomenda_tabela : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EncomendasTipoEncomendas",
                c => new
                    {
                        EncomendaId = c.Int(nullable: false),
                        TipoEncomendaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.EncomendaId, t.TipoEncomendaId })
                .ForeignKey("dbo.Encomendas", t => t.EncomendaId)
                .ForeignKey("dbo.TipoEncomendas", t => t.TipoEncomendaId)
                .Index(t => t.EncomendaId)
                .Index(t => t.TipoEncomendaId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EncomendasTipoEncomendas", "TipoEncomendaId", "dbo.TipoEncomendas");
            DropForeignKey("dbo.EncomendasTipoEncomendas", "EncomendaId", "dbo.Encomendas");
            DropIndex("dbo.EncomendasTipoEncomendas", new[] { "TipoEncomendaId" });
            DropIndex("dbo.EncomendasTipoEncomendas", new[] { "EncomendaId" });
            DropTable("dbo.EncomendasTipoEncomendas");
        }
    }
}
