namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addSavSemanaEntrega : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Savs", "SemanaEntrega", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Savs", "SemanaEntrega");
        }
    }
}
