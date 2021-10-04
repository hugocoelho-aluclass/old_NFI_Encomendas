namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addRecolhasAosSavs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Recolhas",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    DataPedidoRecolha = c.DateTime(nullable: false),
                    DataRecolha = c.DateTime(nullable: false),
                    DataChegadaPrevista = c.DateTime(nullable: false),
                    RecolhaCompleta = c.Boolean(nullable: false),
                    EstadoProduto = c.String(),
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.Savs", "TemRecolha", c => c.Boolean(nullable: false));
            AddColumn("dbo.Savs", "Recolha_Id", c => c.Int());
            CreateIndex("dbo.Savs", "Recolha_Id");
            AddForeignKey("dbo.Savs", "Recolha_Id", "dbo.Recolhas", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Savs", "Recolha_Id", "dbo.Recolhas");
            DropIndex("dbo.Savs", new[] { "Recolha_Id" });
            DropColumn("dbo.Savs", "Recolha_Id");
            DropColumn("dbo.Savs", "TemRecolha");
            DropTable("dbo.Recolhas");
        }
    }
}
