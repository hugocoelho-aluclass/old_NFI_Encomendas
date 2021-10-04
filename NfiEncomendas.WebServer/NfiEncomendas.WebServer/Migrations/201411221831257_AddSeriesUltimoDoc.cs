namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddSeriesUltimoDoc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clientes", "UltimoDoc", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Clientes", "UltimoDoc");
        }
    }
}
