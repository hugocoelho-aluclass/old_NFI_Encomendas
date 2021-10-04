namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addRecolha : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Recolhas", "EstadoRecolha_Id", c => c.Int());
            CreateIndex("dbo.Recolhas", "EstadoRecolha_Id");
            AddForeignKey("dbo.Recolhas", "EstadoRecolha_Id", "dbo.EstadoRecolhas", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Recolhas", "EstadoRecolha_Id", "dbo.EstadoRecolhas");
            DropIndex("dbo.Recolhas", new[] { "EstadoRecolha_Id" });
            DropColumn("dbo.Recolhas", "EstadoRecolha_Id");
        }
    }
}
