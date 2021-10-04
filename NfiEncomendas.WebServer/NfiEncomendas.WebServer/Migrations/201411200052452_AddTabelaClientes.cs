namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTabelaClientes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clientes",
                c => new
                {
                    NumCliente = c.Int(nullable: false, identity: true),
                    NomeCliente = c.String(),
                })
                .PrimaryKey(t => t.NumCliente);

        }

        public override void Down()
        {
            DropTable("dbo.Clientes");
        }
    }
}
