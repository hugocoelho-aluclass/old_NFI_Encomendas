namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddEstadoSAV : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Savs", name: "Estado_IdEstadoEncomenda", newName: "Estado_IdEstadoSav");
            RenameIndex(table: "dbo.Savs", name: "IX_Estado_IdEstadoEncomenda", newName: "IX_Estado_IdEstadoSav");
            DropForeignKey("dbo.Savs", "FK_dbo.Savs_dbo.IX_Estado_IdEstadoEncomenda", "dbo.Savs");

            CreateTable(
                "dbo.EstadoSavs",
                c => new
                {
                    IdEstadoSav = c.Int(nullable: false, identity: true),
                    NomeEstadoSav = c.String(),
                    SubEstado = c.Int(nullable: false),
                    Anulado = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.IdEstadoSav);

            DropColumn("dbo.Savs", "NumVaos");

        }

        public override void Down()
        {
            CreateTable(
                "dbo.EstadoEncomendas",
                c => new
                {
                    IdEstadoEncomenda = c.Int(nullable: false, identity: true),
                    NomeEstadoEncomenda = c.String(),
                    Anulado = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.IdEstadoEncomenda);

            AddColumn("dbo.Savs", "NumVaos", c => c.Int(nullable: false));
            DropTable("dbo.EstadoSavs");
            RenameIndex(table: "dbo.Savs", name: "IX_Estado_IdEstadoSav", newName: "IX_Estado_IdEstadoEncomenda");
            RenameColumn(table: "dbo.Savs", name: "Estado_IdEstadoSav", newName: "Estado_IdEstadoEncomenda");
        }
    }
}
