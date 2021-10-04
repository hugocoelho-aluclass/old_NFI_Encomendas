namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddSavsCampos3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Savs", "DataEntrada");
        }

        public override void Down()
        {
            AddColumn("dbo.Savs", "DataEntrada", c => c.DateTime(nullable: false));
        }
    }
}
