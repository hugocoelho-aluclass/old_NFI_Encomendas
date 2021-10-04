namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddProdutoSAV : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProdutoSavs",
                c => new
                {
                    IdProdutoSav = c.Int(nullable: false, identity: true),
                    NumProdutoSav = c.Int(nullable: false),
                    NomeProdutoSav = c.String(),
                    Anulado = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.IdProdutoSav);

        }

        public override void Down()
        {
            DropTable("dbo.ProdutoSavs");
        }
    }
}
