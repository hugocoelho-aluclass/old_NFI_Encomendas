namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addTabelaRelatorios : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Relatorios",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    NomeUtilizador = c.String(),
                    HtmlQuery = c.String(),
                    Controller = c.String(),
                    Method = c.String(),
                    NomeFicheiro = c.String(),
                    TipoFicheiro = c.String(),
                    UniqueId = c.String(),
                    DataGerado = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.Relatorios");
        }
    }
}
