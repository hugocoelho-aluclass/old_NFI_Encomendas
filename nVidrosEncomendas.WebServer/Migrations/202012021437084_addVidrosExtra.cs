namespace NVidrosEncomendas.WebServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addVidrosExtra : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VidrosExtras",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Num = c.Int(nullable: false),
                        Nome = c.String(),
                        Anulado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.VidrosExtras");
        }
    }
}
