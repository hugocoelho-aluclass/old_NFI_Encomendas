namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UtilizadorPassword : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Operadores", "Password", c => c.String(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Operadores", "Password");
        }
    }
}
