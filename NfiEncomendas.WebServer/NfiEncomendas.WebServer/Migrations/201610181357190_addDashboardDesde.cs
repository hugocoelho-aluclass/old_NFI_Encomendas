namespace NfiEncomendas.WebServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class addDashboardDesde : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Operadores", "DashboardDesde", c => c.DateTime(nullable: false, defaultValue: new DateTime(2016, 01, 01)));
        }

        public override void Down()
        {
            DropColumn("dbo.Operadores", "DashboardDesde");
        }



    }
}
