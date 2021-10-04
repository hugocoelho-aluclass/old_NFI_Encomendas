namespace NVidrosEncomendas.WebServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeSetorEncomendaString : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TipoEncomendas", "Setor");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TipoEncomendas", "Setor", c => c.String());
        }
    }
}
