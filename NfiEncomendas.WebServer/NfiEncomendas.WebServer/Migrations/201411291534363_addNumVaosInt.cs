namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addNumVaosInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Encomendas", "NumVaos", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            AlterColumn("dbo.Encomendas", "NumVaos", c => c.String());
        }
    }
}
