namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddSavsCampos1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Savs", "DireitoNaoConformidade", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Savs", "DireitoNaoConformidade");
        }
    }
}
