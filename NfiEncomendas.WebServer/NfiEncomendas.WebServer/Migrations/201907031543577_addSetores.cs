namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addSetores : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Setors",
                c => new
                {
                    IdSetor = c.Int(nullable: false, identity: true),
                    NumSetor = c.Int(nullable: false),
                    Nome = c.String(),
                    Anulado = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.IdSetor);

        }

        public override void Down()
        {
            DropTable("dbo.Setors");
        }
    }
}
