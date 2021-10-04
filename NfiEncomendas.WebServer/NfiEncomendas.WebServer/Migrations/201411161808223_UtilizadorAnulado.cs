namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UtilizadorAnulado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Operadores", "Anulado", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Operadores", "Anulado");
        }
    }
}
