namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class userDepartamentos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DepartamentoSavs", "Operadores_UtilizadorId", c => c.Int());
            AddColumn("dbo.Operadores", "AdminSav", c => c.Boolean(nullable: false));
            CreateIndex("dbo.DepartamentoSavs", "Operadores_UtilizadorId");
            AddForeignKey("dbo.DepartamentoSavs", "Operadores_UtilizadorId", "dbo.Operadores", "UtilizadorId");
            DropColumn("dbo.Operadores", "FechaSav");
        }

        public override void Down()
        {
            AddColumn("dbo.Operadores", "FechaSav", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.DepartamentoSavs", "Operadores_UtilizadorId", "dbo.Operadores");
            DropIndex("dbo.DepartamentoSavs", new[] { "Operadores_UtilizadorId" });
            DropColumn("dbo.Operadores", "AdminSav");
            DropColumn("dbo.DepartamentoSavs", "Operadores_UtilizadorId");
        }
    }
}
