namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddSavUpdates : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Savs", "OperadorResponsavel_UtilizadorId", "dbo.Operadores");
            DropForeignKey("dbo.Savs", "TipoEncomenda_IdTipoEncomenda", "dbo.TipoEncomendas");
            DropIndex("dbo.Savs", new[] { "OperadorResponsavel_UtilizadorId" });
            DropIndex("dbo.Savs", new[] { "TipoEncomenda_IdTipoEncomenda" });
            AddColumn("dbo.Savs", "Departamento_IdDepartamentoSav", c => c.Int());
            AddColumn("dbo.Savs", "Produto_IdProdutoSav", c => c.Int());
            AddColumn("dbo.Savs", "TipoAvaria_IdTipoAvaria", c => c.Int());
            CreateIndex("dbo.Savs", "Departamento_IdDepartamentoSav");
            CreateIndex("dbo.Savs", "Produto_IdProdutoSav");
            CreateIndex("dbo.Savs", "TipoAvaria_IdTipoAvaria");
            AddForeignKey("dbo.Savs", "Departamento_IdDepartamentoSav", "dbo.DepartamentoSavs", "IdDepartamentoSav");
            AddForeignKey("dbo.Savs", "Produto_IdProdutoSav", "dbo.ProdutoSavs", "IdProdutoSav");
            AddForeignKey("dbo.Savs", "TipoAvaria_IdTipoAvaria", "dbo.TipoAvarias", "IdTipoAvaria");
            DropColumn("dbo.Savs", "RefObra");
            DropColumn("dbo.Savs", "OperadorResponsavel_UtilizadorId");
            DropColumn("dbo.Savs", "TipoEncomenda_IdTipoEncomenda");
        }

        public override void Down()
        {
            AddColumn("dbo.Savs", "TipoEncomenda_IdTipoEncomenda", c => c.Int());
            AddColumn("dbo.Savs", "OperadorResponsavel_UtilizadorId", c => c.Int());
            AddColumn("dbo.Savs", "RefObra", c => c.String());
            DropForeignKey("dbo.Savs", "TipoAvaria_IdTipoAvaria", "dbo.TipoAvarias");
            DropForeignKey("dbo.Savs", "Produto_IdProdutoSav", "dbo.ProdutoSavs");
            DropForeignKey("dbo.Savs", "Departamento_IdDepartamentoSav", "dbo.DepartamentoSavs");
            DropIndex("dbo.Savs", new[] { "TipoAvaria_IdTipoAvaria" });
            DropIndex("dbo.Savs", new[] { "Produto_IdProdutoSav" });
            DropIndex("dbo.Savs", new[] { "Departamento_IdDepartamentoSav" });
            DropColumn("dbo.Savs", "TipoAvaria_IdTipoAvaria");
            DropColumn("dbo.Savs", "Produto_IdProdutoSav");
            DropColumn("dbo.Savs", "Departamento_IdDepartamentoSav");
            CreateIndex("dbo.Savs", "TipoEncomenda_IdTipoEncomenda");
            CreateIndex("dbo.Savs", "OperadorResponsavel_UtilizadorId");
            AddForeignKey("dbo.Savs", "TipoEncomenda_IdTipoEncomenda", "dbo.TipoEncomendas", "IdTipoEncomenda");
            AddForeignKey("dbo.Savs", "OperadorResponsavel_UtilizadorId", "dbo.Operadores", "UtilizadorId");
        }
    }
}
