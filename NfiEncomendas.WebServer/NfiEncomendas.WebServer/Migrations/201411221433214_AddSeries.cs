namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddSeries : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Series",
                c => new
                {
                    NumSerie = c.String(nullable: false, maxLength: 128),
                    NomeSerie = c.String(),
                    Inativa = c.Boolean(nullable: false),
                    SerieDefeito = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.NumSerie);

        }

        public override void Down()
        {
            DropTable("dbo.Series");
        }
    }
}
