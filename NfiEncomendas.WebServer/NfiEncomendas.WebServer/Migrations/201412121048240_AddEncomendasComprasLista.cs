namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddEncomendasComprasLista : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.EncomendasCompras", name: "Encomenda_IdEncomenda", newName: "Encomendas_IdEncomenda");
            RenameIndex(table: "dbo.EncomendasCompras", name: "IX_Encomenda_IdEncomenda", newName: "IX_Encomendas_IdEncomenda");
        }

        public override void Down()
        {
            RenameIndex(table: "dbo.EncomendasCompras", name: "IX_Encomendas_IdEncomenda", newName: "IX_Encomenda_IdEncomenda");
            RenameColumn(table: "dbo.EncomendasCompras", name: "Encomendas_IdEncomenda", newName: "Encomenda_IdEncomenda");
        }
    }
}
