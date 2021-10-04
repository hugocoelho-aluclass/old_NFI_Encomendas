namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddAlterarNumIdsClientes : DbMigration
    {
        public override void Up()
        {
            //DropPrimaryKey("dbo.Clientes");
            //AddColumn("dbo.Clientes", "IdCliente", c => c.Int(nullable: false, identity: true));
            //AddPrimaryKey("dbo.Clientes", "IdCliente");
            //DropColumn("dbo.Clientes", "NumCliente");
            DropTable("dbo.Clientes");
            CreateTable(
                "dbo.Clientes",
                c => new
                {
                    IdCliente = c.Int(nullable: false, identity: true),
                    NomeCliente = c.String(),
                })
                .PrimaryKey(t => t.IdCliente);
            AddColumn("dbo.Clientes", "Anulado", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            AddColumn("dbo.Clientes", "NumCliente", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.Clientes");
            DropColumn("dbo.Clientes", "IdCliente");
            AddPrimaryKey("dbo.Clientes", "NumCliente");
        }
    }
}
