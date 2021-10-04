namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class SavAddInfoExtra : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Savs", "TipoAvariaExtra", c => c.String());
            AddColumn("dbo.TipoAvarias", "InfoExtra", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.TipoAvarias", "InfoExtra");
            DropColumn("dbo.Savs", "TipoAvariaExtra");
        }
    }
}
