namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddAnexosESavsUpd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Anexos",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    NomeFicheiro = c.String(),
                    Anulado = c.Boolean(nullable: false),
                    Savs_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Savs", t => t.Savs_Id)
                .Index(t => t.Savs_Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Anexos", "Savs_Id", "dbo.Savs");
            DropIndex("dbo.Anexos", new[] { "Savs_Id" });
            DropTable("dbo.Anexos");
        }
    }
}
