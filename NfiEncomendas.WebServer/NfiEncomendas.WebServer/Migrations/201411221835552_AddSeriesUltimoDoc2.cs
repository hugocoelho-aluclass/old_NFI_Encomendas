namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddSeriesUltimoDoc2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Series", "UltimoDoc", c => c.Int(nullable: false));
            DropColumn("dbo.Clientes", "UltimoDoc");
        }

        public override void Down()
        {
            AddColumn("dbo.Clientes", "UltimoDoc", c => c.Int(nullable: false));
            DropColumn("dbo.Series", "UltimoDoc");
        }
    }
}
