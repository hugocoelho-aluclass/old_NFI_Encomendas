namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addEncomendaNumSerie : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Encomendas", "NumSerieEncomenda", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Encomendas", "NumSerieEncomenda");
        }
    }
}
