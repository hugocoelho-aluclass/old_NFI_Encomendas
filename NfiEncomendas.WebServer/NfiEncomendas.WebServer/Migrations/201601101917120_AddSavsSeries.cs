namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddSavsSeries : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Savs", "NumDoc", c => c.Int(nullable: false));
            AddColumn("dbo.Savs", "SerieDoc_NumSerie", c => c.String(maxLength: 128));
            CreateIndex("dbo.Savs", "SerieDoc_NumSerie");
            AddForeignKey("dbo.Savs", "SerieDoc_NumSerie", "dbo.Series", "NumSerie");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Savs", "SerieDoc_NumSerie", "dbo.Series");
            DropIndex("dbo.Savs", new[] { "SerieDoc_NumSerie" });
            DropColumn("dbo.Savs", "SerieDoc_NumSerie");
            DropColumn("dbo.Savs", "NumDoc");
        }
    }
}
