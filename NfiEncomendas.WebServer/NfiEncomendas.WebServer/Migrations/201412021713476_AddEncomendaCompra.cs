namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddEncomendaCompra : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Encomendas", "MaterialEncomendado", c => c.Boolean(nullable: false));
            AddColumn("dbo.Encomendas", "EncomendaMaterialDetalhe", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Encomendas", "EncomendaMaterialDetalhe");
            DropColumn("dbo.Encomendas", "MaterialEncomendado");
        }
    }
}
