namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addEncomendasCompras21 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EncomendasCompras", "NotasFornecedor", c => c.String());
            AddColumn("dbo.EncomendasCompras", "DataPedido", c => c.DateTime(nullable: false));
            DropColumn("dbo.EncomendasCompras", "Detalhe");
        }

        public override void Down()
        {
            AddColumn("dbo.EncomendasCompras", "Detalhe", c => c.String());
            DropColumn("dbo.EncomendasCompras", "DataPedido");
            DropColumn("dbo.EncomendasCompras", "NotasFornecedor");
        }
    }
}
