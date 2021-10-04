namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddClienteAnulado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clientes", "Anulado", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Clientes", "Anulado");
        }
    }
}
