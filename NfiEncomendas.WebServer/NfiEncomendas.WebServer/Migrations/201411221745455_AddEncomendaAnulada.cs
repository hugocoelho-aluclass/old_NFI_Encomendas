namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddEncomendaAnulada : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Encomendas", "Anulada", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Encomendas", "Anulada");
        }
    }
}
