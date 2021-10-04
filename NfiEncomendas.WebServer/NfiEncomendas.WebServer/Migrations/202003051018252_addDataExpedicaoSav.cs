namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addDataExpedicaoSav : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Savs", "DataExpedicao", c => c.DateTime());
        }

        public override void Down()
        {
            DropColumn("dbo.Savs", "DataExpedicao");
        }
    }
}
