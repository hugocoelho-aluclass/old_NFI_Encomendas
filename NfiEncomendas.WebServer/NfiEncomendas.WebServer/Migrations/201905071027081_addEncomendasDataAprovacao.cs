namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addEncomendasDataAprovacao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Encomendas", "DataAprovacao", c => c.DateTime());
        }

        public override void Down()
        {
            DropColumn("dbo.Encomendas", "DataAprovacao");
        }
    }
}
