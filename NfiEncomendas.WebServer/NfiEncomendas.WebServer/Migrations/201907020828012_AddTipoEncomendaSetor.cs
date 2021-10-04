namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTipoEncomendaSetor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TipoEncomendas", "Sector", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.TipoEncomendas", "Sector");
        }
    }
}
