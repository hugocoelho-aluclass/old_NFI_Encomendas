namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddOperadorSAV : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Operadores", "Sav", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Operadores", "Sav");
        }
    }
}
