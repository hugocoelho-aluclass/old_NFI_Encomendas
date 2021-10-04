namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddAlterarNumIds : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Encomendas", "Cliente_NumCliente", "dbo.Clientes");
            DropForeignKey("dbo.Encomendas", "TipoEncomenda_NumTipoEncomenda", "dbo.TipoEncomendas");
            DropIndex("dbo.Encomendas", new[] { "Cliente_NumCliente" });
            DropIndex("dbo.Encomendas", new[] { "TipoEncomenda_NumTipoEncomenda" });
            DropColumn("dbo.Encomendas", "Cliente_NumCliente");
            DropColumn("dbo.Encomendas", "TipoEncomenda_NumTipoEncomenda");
        }

        public override void Down()
        {
            AddColumn("dbo.Encomendas", "TipoEncomenda_NumTipoEncomenda", c => c.Int());
            AddColumn("dbo.Encomendas", "Cliente_NumCliente", c => c.Int());
            CreateIndex("dbo.Encomendas", "TipoEncomenda_NumTipoEncomenda");
            CreateIndex("dbo.Encomendas", "Cliente_NumCliente");
            AddForeignKey("dbo.Encomendas", "TipoEncomenda_NumTipoEncomenda", "dbo.TipoEncomendas", "NumTipoEncomenda");
            AddForeignKey("dbo.Encomendas", "Cliente_NumCliente", "dbo.Clientes", "NumCliente");
        }
    }
}
