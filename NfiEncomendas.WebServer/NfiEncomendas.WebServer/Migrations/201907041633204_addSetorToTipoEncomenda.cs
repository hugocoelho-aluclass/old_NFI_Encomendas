namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addSetorToTipoEncomenda : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TipoEncomendas", "SetorEncomenda_IdSetor", c => c.Int());
            CreateIndex("dbo.TipoEncomendas", "SetorEncomenda_IdSetor");
            AddForeignKey("dbo.TipoEncomendas", "SetorEncomenda_IdSetor", "dbo.Setors", "IdSetor");
        }

        public override void Down()
        {
            DropForeignKey("dbo.TipoEncomendas", "SetorEncomenda_IdSetor", "dbo.Setors");
            DropIndex("dbo.TipoEncomendas", new[] { "SetorEncomenda_IdSetor" });
            DropColumn("dbo.TipoEncomendas", "SetorEncomenda_IdSetor");
        }
    }
}
