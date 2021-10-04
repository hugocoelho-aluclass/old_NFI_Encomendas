namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addEncomendasCompras4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EncomendasCompras", "Material", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.EncomendasCompras", "Material");
        }
    }
}
