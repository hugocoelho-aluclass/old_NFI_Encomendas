namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddOperadores : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Operadores",
                c => new
                {
                    UtilizadorId = c.Int(nullable: false, identity: true),
                    AspIdentityId = c.String(),
                    NomeCompleto = c.String(nullable: false, maxLength: 100),
                    NomeLogin = c.String(nullable: false, maxLength: 15),
                    Email = c.String(nullable: false, maxLength: 200),
                    ImagemAvatar = c.String(nullable: false, maxLength: 50),
                    Ativo = c.Boolean(nullable: false),
                    Admin = c.Boolean(nullable: false),
                    CriadoData = c.DateTime(nullable: false),
                    EditadoData = c.DateTime(nullable: false),
                    EditadoPorId = c.Int(),
                    EditadoPor_UtilizadorId = c.Int(),
                })
                .PrimaryKey(t => t.UtilizadorId)
                .ForeignKey("dbo.Operadores", t => t.EditadoPorId)
                .ForeignKey("dbo.Operadores", t => t.EditadoPor_UtilizadorId)
                .Index(t => t.EditadoPorId)
                .Index(t => t.EditadoPor_UtilizadorId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Operadores", "EditadoPor_UtilizadorId", "dbo.Operadores");
            DropForeignKey("dbo.Operadores", "EditadoPorId", "dbo.Operadores");
            DropIndex("dbo.Operadores", new[] { "EditadoPor_UtilizadorId" });
            DropIndex("dbo.Operadores", new[] { "EditadoPorId" });
            DropTable("dbo.Operadores");
        }
    }
}
