namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddEncomendasCompras : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EncomendasCompras",
                c => new
                {
                    IdCompraEncomendas = c.Int(nullable: false, identity: true),
                    Detalhe = c.String(),
                    LinhaCompra = c.Int(nullable: false),
                    DataEntrega = c.DateTime(nullable: false),
                    Encomenda_IdEncomenda = c.Int(),
                })
                .PrimaryKey(t => t.IdCompraEncomendas)
                .ForeignKey("dbo.Encomendas", t => t.Encomenda_IdEncomenda)
                .Index(t => t.Encomenda_IdEncomenda);

        }

        public override void Down()
        {
            DropForeignKey("dbo.EncomendasCompras", "Encomenda_IdEncomenda", "dbo.Encomendas");
            DropIndex("dbo.EncomendasCompras", new[] { "Encomenda_IdEncomenda" });
            DropTable("dbo.EncomendasCompras");
        }
    }
}
