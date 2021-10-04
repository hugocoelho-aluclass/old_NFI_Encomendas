namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddNotasResolvida : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Savs", "NotasResolvida", c => c.String());
            //DropTable("dbo.EstadoEncomendas");

        }

        public override void Down()
        {
            DropColumn("dbo.Savs", "NotasResolvida");
        }
    }
}
