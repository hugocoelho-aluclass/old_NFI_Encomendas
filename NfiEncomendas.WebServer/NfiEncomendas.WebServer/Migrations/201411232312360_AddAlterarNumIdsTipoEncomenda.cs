namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddAlterarNumIdsTipoEncomenda : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.TipoEncomendas");
            CreateTable(
                "dbo.TipoEncomendas",
                c => new
                {
                    IdTipoEncomenda = c.Int(nullable: false, identity: true),
                    NomeTipoEncomenda = c.String(),
                    Anulado = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.IdTipoEncomenda);
            //DropPrimaryKey("dbo.TipoEncomendas");
            //AddColumn("dbo.TipoEncomendas", "IdTipoEncomenda", c => c.Int(nullable: false, identity: true));
            //AddPrimaryKey("dbo.TipoEncomendas", "IdTipoEncomenda");
            //DropColumn("dbo.TipoEncomendas", "NumTipoEncomenda");
        }

        public override void Down()
        {
            AddColumn("dbo.TipoEncomendas", "NumTipoEncomenda", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.TipoEncomendas");
            DropColumn("dbo.TipoEncomendas", "IdTipoEncomenda");
            AddPrimaryKey("dbo.TipoEncomendas", "NumTipoEncomenda");
        }
    }
}
