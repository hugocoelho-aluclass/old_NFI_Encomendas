namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddEncomendasCampos1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Encomendas", "DataExpedidoString", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Encomendas", "DataExpedidoString");
        }
    }
}
