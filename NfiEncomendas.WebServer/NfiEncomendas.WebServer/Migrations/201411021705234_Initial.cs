namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Applications",
                c => new
                {
                    ApplicationId = c.Guid(nullable: false),
                    ApplicationName = c.String(nullable: false, maxLength: 235),
                    Description = c.String(maxLength: 256),
                })
                .PrimaryKey(t => t.ApplicationId);

            CreateTable(
                "dbo.Memberships",
                c => new
                {
                    UserId = c.Guid(nullable: false),
                    ApplicationId = c.Guid(nullable: false),
                    Password = c.String(nullable: false, maxLength: 128),
                    PasswordFormat = c.Int(nullable: false),
                    PasswordSalt = c.String(nullable: false, maxLength: 128),
                    Email = c.String(maxLength: 256),
                    PasswordQuestion = c.String(maxLength: 256),
                    PasswordAnswer = c.String(maxLength: 128),
                    IsApproved = c.Boolean(nullable: false),
                    IsLockedOut = c.Boolean(nullable: false),
                    CreateDate = c.DateTime(nullable: false),
                    LastLoginDate = c.DateTime(nullable: false),
                    LastPasswordChangedDate = c.DateTime(nullable: false),
                    LastLockoutDate = c.DateTime(nullable: false),
                    FailedPasswordAttemptCount = c.Int(nullable: false),
                    FailedPasswordAttemptWindowStart = c.DateTime(nullable: false),
                    FailedPasswordAnswerAttemptCount = c.Int(nullable: false),
                    FailedPasswordAnswerAttemptWindowsStart = c.DateTime(nullable: false),
                    Comment = c.String(maxLength: 256),
                })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.Applications", t => t.ApplicationId)
                .Index(t => t.UserId)
                .Index(t => t.ApplicationId);

            CreateTable(
                "dbo.Users",
                c => new
                {
                    UserId = c.Guid(nullable: false),
                    ApplicationId = c.Guid(nullable: false),
                    UserName = c.String(nullable: false, maxLength: 50),
                    IsAnonymous = c.Boolean(nullable: false),
                    LastActivityDate = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Applications", t => t.ApplicationId)
                .Index(t => t.ApplicationId);

            CreateTable(
                "dbo.Profiles",
                c => new
                {
                    UserId = c.Guid(nullable: false),
                    PropertyNames = c.String(nullable: false, maxLength: 4000),
                    PropertyValueStrings = c.String(nullable: false, maxLength: 4000),
                    PropertyValueBinary = c.Binary(nullable: false, storeType: "image"),
                    LastUpdatedDate = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.Roles",
                c => new
                {
                    RoleId = c.Guid(nullable: false),
                    ApplicationId = c.Guid(nullable: false),
                    RoleName = c.String(nullable: false, maxLength: 256),
                    Description = c.String(maxLength: 256),
                })
                .PrimaryKey(t => t.RoleId)
                .ForeignKey("dbo.Applications", t => t.ApplicationId)
                .Index(t => t.ApplicationId);

            CreateTable(
                "dbo.IdentityUserLogins",
                c => new
                {
                    UserId = c.String(nullable: false, maxLength: 128),
                    LoginProvider = c.String(),
                    ProviderKey = c.String(),
                })
                .PrimaryKey(t => t.UserId);

            CreateTable(
                "dbo.IdentityRoles",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    Name = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.IdentityUserRoles",
                c => new
                {
                    RoleId = c.String(nullable: false, maxLength: 128),
                    UserId = c.String(nullable: false, maxLength: 128),
                    IdentityRole_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.IdentityRoles", t => t.IdentityRole_Id)
                .Index(t => t.IdentityRole_Id);

            CreateTable(
                "dbo.UsersInRoles",
                c => new
                {
                    RoleId = c.Guid(nullable: false),
                    UserId = c.Guid(nullable: false),
                })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.IdentityUserRoles", "IdentityRole_Id", "dbo.IdentityRoles");
            DropForeignKey("dbo.Users", "ApplicationId", "dbo.Applications");
            DropForeignKey("dbo.Roles", "ApplicationId", "dbo.Applications");
            DropForeignKey("dbo.Memberships", "ApplicationId", "dbo.Applications");
            DropForeignKey("dbo.UsersInRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UsersInRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Profiles", "UserId", "dbo.Users");
            DropForeignKey("dbo.Memberships", "UserId", "dbo.Users");
            DropIndex("dbo.UsersInRoles", new[] { "UserId" });
            DropIndex("dbo.UsersInRoles", new[] { "RoleId" });
            DropIndex("dbo.IdentityUserRoles", new[] { "IdentityRole_Id" });
            DropIndex("dbo.Roles", new[] { "ApplicationId" });
            DropIndex("dbo.Profiles", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "ApplicationId" });
            DropIndex("dbo.Memberships", new[] { "ApplicationId" });
            DropIndex("dbo.Memberships", new[] { "UserId" });
            DropTable("dbo.UsersInRoles");
            DropTable("dbo.IdentityUserRoles");
            DropTable("dbo.IdentityRoles");
            DropTable("dbo.IdentityUserLogins");
            DropTable("dbo.Roles");
            DropTable("dbo.Profiles");
            DropTable("dbo.Users");
            DropTable("dbo.Memberships");
            DropTable("dbo.Applications");
        }
    }
}
