namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class userDepartamentos3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DepartamentoSavs", "Operadores_UtilizadorId", "dbo.Operadores");
            DropIndex("dbo.DepartamentoSavs", new[] { "Operadores_UtilizadorId" });
            CreateTable(
                "dbo.OperadoresDepartamentoSavs",
                c => new
                {
                    Operadores_UtilizadorId = c.Int(nullable: false),
                    DepartamentoSav_IdDepartamentoSav = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.Operadores_UtilizadorId, t.DepartamentoSav_IdDepartamentoSav })
                .ForeignKey("dbo.Operadores", t => t.Operadores_UtilizadorId, cascadeDelete: true)
                .ForeignKey("dbo.DepartamentoSavs", t => t.DepartamentoSav_IdDepartamentoSav, cascadeDelete: true)
                .Index(t => t.Operadores_UtilizadorId)
                .Index(t => t.DepartamentoSav_IdDepartamentoSav);

            DropColumn("dbo.DepartamentoSavs", "Operadores_UtilizadorId");
        }

        public override void Down()
        {
            AddColumn("dbo.DepartamentoSavs", "Operadores_UtilizadorId", c => c.Int());
            DropForeignKey("dbo.OperadoresDepartamentoSavs", "DepartamentoSav_IdDepartamentoSav", "dbo.DepartamentoSavs");
            DropForeignKey("dbo.OperadoresDepartamentoSavs", "Operadores_UtilizadorId", "dbo.Operadores");
            DropIndex("dbo.OperadoresDepartamentoSavs", new[] { "DepartamentoSav_IdDepartamentoSav" });
            DropIndex("dbo.OperadoresDepartamentoSavs", new[] { "Operadores_UtilizadorId" });
            DropTable("dbo.OperadoresDepartamentoSavs");
            CreateIndex("dbo.DepartamentoSavs", "Operadores_UtilizadorId");
            AddForeignKey("dbo.DepartamentoSavs", "Operadores_UtilizadorId", "dbo.Operadores", "UtilizadorId");
        }
    }
}
