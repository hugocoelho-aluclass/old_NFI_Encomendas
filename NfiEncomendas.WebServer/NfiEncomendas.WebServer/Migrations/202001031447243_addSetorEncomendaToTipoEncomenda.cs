namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addSetorEncomendaToTipoEncomenda : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.TipoEncomendas", name: "SetorEncomenda_IdSetor", newName: "SetorEncomenda_IdSetorEncomenda");
            RenameIndex(table: "dbo.TipoEncomendas", name: "IX_SetorEncomenda_IdSetor", newName: "IX_SetorEncomenda_IdSetorEncomenda");
            CreateTable(
                "dbo.SetorEncomendas",
                c => new
                {
                    IdSetorEncomenda = c.Int(nullable: false, identity: true),
                    NumSetor = c.Int(nullable: false),
                    Nome = c.String(),
                    Anulado = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.IdSetorEncomenda);
            //    DropColumn("dbo.TipoEncomendas", "Setor");
        }

        public override void Down()
        {
            DropTable("dbo.SetorEncomendas");
            RenameIndex(table: "dbo.TipoEncomendas", name: "IX_SetorEncomenda_IdSetorEncomenda", newName: "IX_SetorEncomenda_IdSetor");
            RenameColumn(table: "dbo.TipoEncomendas", name: "SetorEncomenda_IdSetorEncomenda", newName: "SetorEncomenda_IdSetor");
        }
    }
}
