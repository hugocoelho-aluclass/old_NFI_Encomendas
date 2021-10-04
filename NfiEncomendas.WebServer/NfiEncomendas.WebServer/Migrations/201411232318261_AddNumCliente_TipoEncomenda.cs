namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddNumCliente_TipoEncomenda : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clientes", "NumCliente", c => c.Int(nullable: false));
            AddColumn("dbo.TipoEncomendas", "NumTipoEncomenda", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.TipoEncomendas", "NumTipoEncomenda");
            DropColumn("dbo.Clientes", "NumCliente");
        }
    }
}
