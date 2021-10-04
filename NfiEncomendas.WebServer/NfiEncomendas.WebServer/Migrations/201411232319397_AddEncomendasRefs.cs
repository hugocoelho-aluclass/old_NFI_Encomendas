namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddEncomendasRefs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Encomendas", "Cliente_IdCliente", c => c.Int());
            AddColumn("dbo.Encomendas", "TipoEncomenda_IdTipoEncomenda", c => c.Int());
            CreateIndex("dbo.Encomendas", "Cliente_IdCliente");
            CreateIndex("dbo.Encomendas", "TipoEncomenda_IdTipoEncomenda");
            AddForeignKey("dbo.Encomendas", "Cliente_IdCliente", "dbo.Clientes", "IdCliente");
            AddForeignKey("dbo.Encomendas", "TipoEncomenda_IdTipoEncomenda", "dbo.TipoEncomendas", "IdTipoEncomenda");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Encomendas", "TipoEncomenda_IdTipoEncomenda", "dbo.TipoEncomendas");
            DropForeignKey("dbo.Encomendas", "Cliente_IdCliente", "dbo.Clientes");
            DropIndex("dbo.Encomendas", new[] { "TipoEncomenda_IdTipoEncomenda" });
            DropIndex("dbo.Encomendas", new[] { "Cliente_IdCliente" });
            DropColumn("dbo.Encomendas", "TipoEncomenda_IdTipoEncomenda");
            DropColumn("dbo.Encomendas", "Cliente_IdCliente");
        }
    }
}
