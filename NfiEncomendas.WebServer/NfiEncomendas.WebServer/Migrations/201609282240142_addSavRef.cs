namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addSavRef : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Savs", "Ref", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Savs", "Ref");
        }
    }
}
