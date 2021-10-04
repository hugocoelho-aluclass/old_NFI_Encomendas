namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addEstadosRecolha : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EstadoRecolhas",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    NomeEstado = c.String(),
                    Cor = c.String(),
                    EstadoFechaRecolha = c.Boolean(nullable: false),
                    Anulado = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.EstadoRecolhas");
        }
    }
}
