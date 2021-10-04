namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class EncomendasComprasBugAdd : DbMigration
    {
        public override void Up()
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
                    Encomendas_IdEncomenda = c.Int(),
                })
                .PrimaryKey(t => t.IdCompraEncomendas)
                .ForeignKey("dbo.Encomendas", t => t.Encomendas_IdEncomenda)
                .Index(t => t.Encomendas_IdEncomenda);

        }

        public override void Down()
        {
            DropForeignKey("dbo.EncomendasCompras", "Encomendas_IdEncomenda", "dbo.Encomendas");
            DropIndex("dbo.EncomendasCompras", new[] { "Encomendas_IdEncomenda" });
            DropTable("dbo.EncomendasCompras");
        }
    }
}
