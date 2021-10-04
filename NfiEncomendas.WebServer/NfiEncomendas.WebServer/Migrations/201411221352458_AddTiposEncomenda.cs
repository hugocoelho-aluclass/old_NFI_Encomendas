namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTiposEncomenda : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TipoEncomendas",
                c => new
                {
                    NumTipoEncomenda = c.Int(nullable: false, identity: true),
                    NomeTipoEncomenda = c.String(),
                    Anulado = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.NumTipoEncomenda);

        }

        public override void Down()
        {
            DropTable("dbo.TipoEncomendas");
        }
    }
}
