namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddSavCustos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Savs", "Custos", c => c.Double());
            AddColumn("dbo.Savs", "CustosDescricao", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Savs", "CustosDescricao");
            DropColumn("dbo.Savs", "Custos");
        }
    }
}
