namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddEstadoAvaria : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TipoAvarias",
                c => new
                {
                    IdTipoAvaria = c.Int(nullable: false, identity: true),
                    NumTipoAvaria = c.Int(nullable: false),
                    NomeTipoAvaria = c.String(),
                    Anulado = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.IdTipoAvaria);

            AddColumn("dbo.Operadores", "FechaSav", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Operadores", "FechaSav");
            DropTable("dbo.TipoAvarias");
        }
    }
}
