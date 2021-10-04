namespace NfiEncomendas.WebServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Identity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Memberships", "UserId", "dbo.Users");
            DropForeignKey("dbo.Profiles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UsersInRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.UsersInRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.Memberships", "ApplicationId", "dbo.Applications");
            DropForeignKey("dbo.Roles", "ApplicationId", "dbo.Applications");
            DropForeignKey("dbo.Users", "ApplicationId", "dbo.Applications");
            DropForeignKey("dbo.IdentityUserRoles", "IdentityRole_Id", "dbo.IdentityRoles");
            DropIndex("dbo.Memberships", new[] { "UserId" });
            DropIndex("dbo.Memberships", new[] { "ApplicationId" });
            DropIndex("dbo.Users", new[] { "ApplicationId" });
            DropIndex("dbo.Profiles", new[] { "UserId" });
            DropIndex("dbo.Roles", new[] { "ApplicationId" });
            DropIndex("dbo.IdentityUserRoles", new[] { "IdentityRole_Id" });
            DropIndex("dbo.UsersInRoles", new[] { "RoleId" });
            DropIndex("dbo.UsersInRoles", new[] { "UserId" });
            CreateTable(
                "dbo.Clients",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    Secret = c.String(nullable: false),
                    Name = c.String(nullable: false, maxLength: 100),
                    ApplicationType = c.Int(nullable: false),
                    Active = c.Boolean(nullable: false),
                    RefreshTokenLifeTime = c.Int(nullable: false),
                    AllowedOrigin = c.String(maxLength: 100),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.RefreshTokens",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    Subject = c.String(nullable: false, maxLength: 50),
                    ClientId = c.String(nullable: false, maxLength: 50),
                    IssuedUtc = c.DateTime(nullable: false),
                    ExpiresUtc = c.DateTime(nullable: false),
                    ProtectedTicket = c.String(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            DropTable("dbo.Applications");
            DropTable("dbo.Memberships");
            DropTable("dbo.Users");
            DropTable("dbo.Profiles");
            DropTable("dbo.Roles");
            DropTable("dbo.IdentityUserLogins");
            DropTable("dbo.IdentityRoles");
            DropTable("dbo.IdentityUserRoles");
            DropTable("dbo.UsersInRoles");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.UsersInRoles",
                c => new
                {
                    RoleId = c.Guid(nullable: false),
                    UserId = c.Guid(nullable: false),
                })
                .PrimaryKey(t => new { t.RoleId, t.UserId });

            CreateTable(
                "dbo.IdentityUserRoles",
                c => new
                {
                    RoleId = c.String(nullable: false, maxLength: 128),
                    UserId = c.String(nullable: false, maxLength: 128),
                    IdentityRole_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => new { t.RoleId, t.UserId });

            CreateTable(
                "dbo.IdentityRoles",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    Name = c.String(),
                })
                .PrimaryKey(t => t.Id);

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
                "dbo.Roles",
                c => new
                {
                    RoleId = c.Guid(nullable: false),
                    ApplicationId = c.Guid(nullable: false),
                    RoleName = c.String(nullable: false, maxLength: 256),
                    Description = c.String(maxLength: 256),
                })
                .PrimaryKey(t => t.RoleId);

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
                .PrimaryKey(t => t.UserId);

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
                .PrimaryKey(t => t.UserId);

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
                .PrimaryKey(t => t.UserId);

            CreateTable(
                "dbo.Applications",
                c => new
                {
                    ApplicationId = c.Guid(nullable: false),
                    ApplicationName = c.String(nullable: false, maxLength: 235),
                    Description = c.String(maxLength: 256),
                })
                .PrimaryKey(t => t.ApplicationId);

            DropTable("dbo.RefreshTokens");
            DropTable("dbo.Clients");
            CreateIndex("dbo.UsersInRoles", "UserId");
            CreateIndex("dbo.UsersInRoles", "RoleId");
            CreateIndex("dbo.IdentityUserRoles", "IdentityRole_Id");
            CreateIndex("dbo.Roles", "ApplicationId");
            CreateIndex("dbo.Profiles", "UserId");
            CreateIndex("dbo.Users", "ApplicationId");
            CreateIndex("dbo.Memberships", "ApplicationId");
            CreateIndex("dbo.Memberships", "UserId");
            AddForeignKey("dbo.IdentityUserRoles", "IdentityRole_Id", "dbo.IdentityRoles", "Id");
            AddForeignKey("dbo.Users", "ApplicationId", "dbo.Applications", "ApplicationId");
            AddForeignKey("dbo.Roles", "ApplicationId", "dbo.Applications", "ApplicationId");
            AddForeignKey("dbo.Memberships", "ApplicationId", "dbo.Applications", "ApplicationId");
            AddForeignKey("dbo.UsersInRoles", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.UsersInRoles", "RoleId", "dbo.Roles", "RoleId", cascadeDelete: true);
            AddForeignKey("dbo.Profiles", "UserId", "dbo.Users", "UserId");
            AddForeignKey("dbo.Memberships", "UserId", "dbo.Users", "UserId");
        }
    }
}
