namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addSavsNovosCampos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Savs", "Causa", c => c.String());
            AddColumn("dbo.Savs", "AcaoImplementar", c => c.String());
            AddColumn("dbo.Savs", "CustosTransporte", c => c.Double());
            AddColumn("dbo.Savs", "Setor_IdSetor", c => c.Int());
            CreateIndex("dbo.Savs", "Setor_IdSetor");
            AddForeignKey("dbo.Savs", "Setor_IdSetor", "dbo.Setors", "IdSetor");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Savs", "Setor_IdSetor", "dbo.Setors");
            DropIndex("dbo.Savs", new[] { "Setor_IdSetor" });
            DropColumn("dbo.Savs", "Setor_IdSetor");
            DropColumn("dbo.Savs", "CustosTransporte");
            DropColumn("dbo.Savs", "AcaoImplementar");
            DropColumn("dbo.Savs", "Causa");
        }
    }
}
