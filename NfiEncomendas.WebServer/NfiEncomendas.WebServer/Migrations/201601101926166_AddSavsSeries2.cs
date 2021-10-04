namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddSavsSeries2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Series", "UltimoDocSav", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Series", "UltimoDocSav");
        }
    }
}
