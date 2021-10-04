namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddSavsCampos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Savs", "DescricaoSav", c => c.String());
            AddColumn("dbo.Savs", "DataSav", c => c.DateTime(nullable: false));
            AddColumn("dbo.Savs", "DataRespostaAoCliente", c => c.DateTime());
            AddColumn("dbo.Savs", "RespostaAoCliente", c => c.String());
            AddColumn("dbo.Savs", "CriadoData", c => c.DateTime(nullable: false));
            AddColumn("dbo.Savs", "EditadoData", c => c.DateTime(nullable: false));
            AddColumn("dbo.Savs", "CriadoPor_UtilizadorId", c => c.Int());
            AddColumn("dbo.Savs", "EditadoPor_UtilizadorId", c => c.Int());
            CreateIndex("dbo.Savs", "CriadoPor_UtilizadorId");
            CreateIndex("dbo.Savs", "EditadoPor_UtilizadorId");
            AddForeignKey("dbo.Savs", "CriadoPor_UtilizadorId", "dbo.Operadores", "UtilizadorId");
            AddForeignKey("dbo.Savs", "EditadoPor_UtilizadorId", "dbo.Operadores", "UtilizadorId");
            DropColumn("dbo.Savs", "RefSav");
            DropColumn("dbo.Savs", "DataEncomenda");
        }

        public override void Down()
        {
            AddColumn("dbo.Savs", "DataEncomenda", c => c.DateTime(nullable: false));
            AddColumn("dbo.Savs", "RefSav", c => c.String());
            DropForeignKey("dbo.Savs", "EditadoPor_UtilizadorId", "dbo.Operadores");
            DropForeignKey("dbo.Savs", "CriadoPor_UtilizadorId", "dbo.Operadores");
            DropIndex("dbo.Savs", new[] { "EditadoPor_UtilizadorId" });
            DropIndex("dbo.Savs", new[] { "CriadoPor_UtilizadorId" });
            DropColumn("dbo.Savs", "EditadoPor_UtilizadorId");
            DropColumn("dbo.Savs", "CriadoPor_UtilizadorId");
            DropColumn("dbo.Savs", "EditadoData");
            DropColumn("dbo.Savs", "CriadoData");
            DropColumn("dbo.Savs", "RespostaAoCliente");
            DropColumn("dbo.Savs", "DataRespostaAoCliente");
            DropColumn("dbo.Savs", "DataSav");
            DropColumn("dbo.Savs", "DescricaoSav");
        }
    }
}
