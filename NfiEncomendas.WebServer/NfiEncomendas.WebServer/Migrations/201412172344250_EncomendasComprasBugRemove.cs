namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class EncomendasComprasBugRemove : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EncomendasCompras", "Encomenda_IdEncomenda", "dbo.Encomendas");
            DropIndex("dbo.EncomendasCompras", new[] { "Encomenda_IdEncomenda" });
            DropTable("dbo.EncomendasCompras");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.EncomendasCompras",
                c => new
                {
                    IdCompraEncomendas = c.Int(nullable: false, identity: true),
                    Material = c.String(),
                    NotasFornecedor = c.String(),
                    LinhaCompra = c.Int(nullable: false),
                    DataEntrega = c.DateTime(),
                    DataPedido = c.DateTime(),
                    Encomenda_IdEncomenda = c.Int(),
                })
                .PrimaryKey(t => t.IdCompraEncomendas);

            CreateIndex("dbo.EncomendasCompras", "Encomenda_IdEncomenda");
            AddForeignKey("dbo.EncomendasCompras", "Encomenda_IdEncomenda", "dbo.Encomendas", "IdEncomenda");
        }
    }
}
