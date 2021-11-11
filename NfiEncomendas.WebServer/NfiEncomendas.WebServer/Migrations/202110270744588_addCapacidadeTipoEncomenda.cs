namespace NfiEncomendas.WebServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCapacidadeTipoEncomenda : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TipoEncomendas", "CapacidadePrix", c => c.Int());
            AddColumn("dbo.TipoEncomendas", "CapacidadeWis", c => c.Int());
            AddColumn("dbo.TipoEncomendas", "CapacidadeResto", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TipoEncomendas", "CapacidadeResto");
            DropColumn("dbo.TipoEncomendas", "CapacidadeWis");
            DropColumn("dbo.TipoEncomendas", "CapacidadePrix");
        }
    }
}
