namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddMarcarRespostaClienteSAV : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Savs", "MarcarRespostaAoCliente", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Savs", "MarcarRespostaAoCliente");
        }
    }
}
