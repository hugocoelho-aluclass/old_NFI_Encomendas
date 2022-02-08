namespace NfiEncomendas.WebServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addProblemas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Problemas",
                c => new
                    {
                        IdProblema = c.Int(nullable: false, identity: true),
                        Nome = c.String(maxLength: 50, nullable: false),
                        Descricao = c.String(nullable: false),
                        DescricaoCausa = c.String(nullable: false),
                        Acompanhamento = c.String(),
                        AcaoImplementar = c.String(),
                        DataCriacao = c.DateTime(nullable: false),
                        Eficacia = c.Int(),
                        AvaliacaoEficacia = c.String(),
                        Fechado = c.Boolean(nullable: false),
                        DataAvaliacao = c.DateTime(),
                        IdAnterior = c.Int(),
                        Departamento_IdDepartamentoSav = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdProblema)
                .ForeignKey("dbo.DepartamentoSavs", t => t.Departamento_IdDepartamentoSav)
                .Index(t => t.Departamento_IdDepartamentoSav);
            
            AddColumn("dbo.Savs", "Garantia", c => c.Boolean(nullable: false));
            AddColumn("dbo.Savs", "Problema_IdProblema", c => c.Int());
            CreateIndex("dbo.Savs", "Problema_IdProblema");
            AddForeignKey("dbo.Savs", "Problema_IdProblema", "dbo.Problemas", "IdProblema");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Savs", "Problema_IdProblema", "dbo.Problemas");
            DropForeignKey("dbo.Problemas", "Departamento_IdDepartamentoSav", "dbo.DepartamentoSavs");
            DropIndex("dbo.Savs", new[] { "Problema_IdProblema" });
            DropIndex("dbo.Problemas", new[] { "Departamento_IdDepartamentoSav" });
            DropColumn("dbo.Savs", "Problema_IdProblema");
            DropColumn("dbo.Savs", "Garantia");
            DropTable("dbo.Problemas");
        }
    }
}
