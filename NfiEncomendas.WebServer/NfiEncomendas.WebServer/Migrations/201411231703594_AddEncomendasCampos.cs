namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddEncomendasCampos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Encomendas", "NumVaos", c => c.String());
            AddColumn("dbo.Encomendas", "Estado", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Encomendas", "Estado");
            DropColumn("dbo.Encomendas", "NumVaos");
        }
    }
}
