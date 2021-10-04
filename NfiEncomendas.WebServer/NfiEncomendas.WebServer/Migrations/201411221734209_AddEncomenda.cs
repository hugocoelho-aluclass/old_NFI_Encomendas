namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddEncomenda : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Encomendas",
                c => new
                {
                    IdEncomenda = c.Int(nullable: false, identity: true),
                    NumDoc = c.Int(nullable: false),
                    NomeArtigo = c.String(),
                    Cor = c.String(),
                    Painel = c.String(),
                    Producao = c.String(),
                    SemanaEntrega = c.Int(nullable: false),
                    DataPedido = c.DateTime(nullable: false),
                    DataExpedido = c.DateTime(nullable: false),
                    Fatura = c.String(),
                    Notas = c.String(),
                    Cliente_NumCliente = c.Int(),
                    SerieDoc_NumSerie = c.String(maxLength: 128),
                    TipoEncomenda_NumTipoEncomenda = c.Int(),
                })
                .PrimaryKey(t => t.IdEncomenda)
                .ForeignKey("dbo.Clientes", t => t.Cliente_NumCliente)
                .ForeignKey("dbo.Series", t => t.SerieDoc_NumSerie)
                .ForeignKey("dbo.TipoEncomendas", t => t.TipoEncomenda_NumTipoEncomenda)
                .Index(t => t.Cliente_NumCliente)
                .Index(t => t.SerieDoc_NumSerie)
                .Index(t => t.TipoEncomenda_NumTipoEncomenda);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Encomendas", "TipoEncomenda_NumTipoEncomenda", "dbo.TipoEncomendas");
            DropForeignKey("dbo.Encomendas", "SerieDoc_NumSerie", "dbo.Series");
            DropForeignKey("dbo.Encomendas", "Cliente_NumCliente", "dbo.Clientes");
            DropIndex("dbo.Encomendas", new[] { "TipoEncomenda_NumTipoEncomenda" });
            DropIndex("dbo.Encomendas", new[] { "SerieDoc_NumSerie" });
            DropIndex("dbo.Encomendas", new[] { "Cliente_NumCliente" });
            DropTable("dbo.Encomendas");
        }
    }
}
