namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addSAVS : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EstadoEncomendas",
                c => new
                {
                    IdEstadoEncomenda = c.Int(nullable: false, identity: true),
                    NumEstadoEncomenda = c.Int(nullable: false),
                    NomeEstadoEncomenda = c.String(),
                    Anulado = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.IdEstadoEncomenda);

            CreateTable(
                "dbo.Savs",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    RefSav = c.String(),
                    RefObra = c.String(),
                    DataEncomenda = c.DateTime(nullable: false),
                    DataEntrada = c.DateTime(nullable: false),
                    DataEstado = c.DateTime(nullable: false),
                    NumVaos = c.Int(nullable: false),
                    MarcarResolvida = c.Boolean(nullable: false),
                    DataResolvida = c.DateTime(nullable: false),
                    NotasAdicionais = c.String(),
                    Anulada = c.Boolean(nullable: false),
                    Cliente_IdCliente = c.Int(),
                    Estado_IdEstadoEncomenda = c.Int(),
                    OperadorResponsavel_UtilizadorId = c.Int(),
                    TipoEncomenda_IdTipoEncomenda = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clientes", t => t.Cliente_IdCliente)
                .ForeignKey("dbo.EstadoEncomendas", t => t.Estado_IdEstadoEncomenda)
                .ForeignKey("dbo.Operadores", t => t.OperadorResponsavel_UtilizadorId)
                .ForeignKey("dbo.TipoEncomendas", t => t.TipoEncomenda_IdTipoEncomenda)
                .Index(t => t.Cliente_IdCliente)
                .Index(t => t.Estado_IdEstadoEncomenda)
                .Index(t => t.OperadorResponsavel_UtilizadorId)
                .Index(t => t.TipoEncomenda_IdTipoEncomenda);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Savs", "TipoEncomenda_IdTipoEncomenda", "dbo.TipoEncomendas");
            DropForeignKey("dbo.Savs", "OperadorResponsavel_UtilizadorId", "dbo.Operadores");
            DropForeignKey("dbo.Savs", "Estado_IdEstadoEncomenda", "dbo.EstadoEncomendas");
            DropForeignKey("dbo.Savs", "Cliente_IdCliente", "dbo.Clientes");
            DropIndex("dbo.Savs", new[] { "TipoEncomenda_IdTipoEncomenda" });
            DropIndex("dbo.Savs", new[] { "OperadorResponsavel_UtilizadorId" });
            DropIndex("dbo.Savs", new[] { "Estado_IdEstadoEncomenda" });
            DropIndex("dbo.Savs", new[] { "Cliente_IdCliente" });
            DropTable("dbo.Savs");
            DropTable("dbo.EstadoEncomendas");
        }
    }
}
