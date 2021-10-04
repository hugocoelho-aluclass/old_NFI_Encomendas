namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addUserComercial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Operadores", "Comercial", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Operadores", "Comercial");
        }
    }
}
