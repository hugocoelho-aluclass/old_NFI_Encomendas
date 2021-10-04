namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddMarcarRespostaClienteSAV1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstadoSavs", "PreSeleccionadoNaPesquisa", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.EstadoSavs", "PreSeleccionadoNaPesquisa");
        }
    }
}
