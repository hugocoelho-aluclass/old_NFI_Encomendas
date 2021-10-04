namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Utilizador_ImagemAvatar : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Operadores", "ImagemAvatar", c => c.String(maxLength: 50));
        }

        public override void Down()
        {
            AlterColumn("dbo.Operadores", "ImagemAvatar", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
