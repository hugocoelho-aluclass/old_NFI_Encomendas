namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTipoEncomendaSetor2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TipoEncomendas", "Setor", c => c.String());
            DropColumn("dbo.TipoEncomendas", "Sector");
        }

        public override void Down()
        {
            AddColumn("dbo.TipoEncomendas", "Sector", c => c.String());
            DropColumn("dbo.TipoEncomendas", "Setor");
        }
    }
}
