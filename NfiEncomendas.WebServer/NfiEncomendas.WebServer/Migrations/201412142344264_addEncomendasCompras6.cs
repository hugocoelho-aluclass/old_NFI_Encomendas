namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addEncomendasCompras6 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EncomendasCompras", "DataEntrega", c => c.DateTime());
            AlterColumn("dbo.EncomendasCompras", "DataPedido", c => c.DateTime());
        }

        public override void Down()
        {
            AlterColumn("dbo.EncomendasCompras", "DataPedido", c => c.DateTime(nullable: false));
            AlterColumn("dbo.EncomendasCompras", "DataEntrega", c => c.DateTime(nullable: false));
        }
    }
}
