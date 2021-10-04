namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddEncomendaCompra1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Encomendas", "MaterialEncomendadoDetalhe", c => c.String());
            DropColumn("dbo.Encomendas", "EncomendaMaterialDetalhe");
        }

        public override void Down()
        {
            AddColumn("dbo.Encomendas", "EncomendaMaterialDetalhe", c => c.String());
            DropColumn("dbo.Encomendas", "MaterialEncomendadoDetalhe");
        }
    }
}
