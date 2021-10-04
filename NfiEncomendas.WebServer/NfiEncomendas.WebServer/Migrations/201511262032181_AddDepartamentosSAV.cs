namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddDepartamentosSAV : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DepartamentoSavs",
                c => new
                {
                    IdDepartamentoSav = c.Int(nullable: false, identity: true),
                    NumDepartamentoSav = c.Int(nullable: false),
                    NomeDepartamentoSav = c.String(),
                    Anulado = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.IdDepartamentoSav);
        }

        public override void Down()
        {
            DropTable("dbo.DepartamentoSavs");
        }
    }
}
