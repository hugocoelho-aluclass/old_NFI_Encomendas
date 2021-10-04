namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UpdEstadoSAV : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstadoSavs", "MarcaEncerrado", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.EstadoSavs", "MarcaEncerrado");
        }
    }
}
