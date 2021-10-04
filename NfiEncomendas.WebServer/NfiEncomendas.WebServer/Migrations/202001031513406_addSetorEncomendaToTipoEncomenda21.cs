namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addSetorEncomendaToTipoEncomenda21 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TipoEncomendas", "Setor", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.TipoEncomendas", "Setor");
        }
    }
}
