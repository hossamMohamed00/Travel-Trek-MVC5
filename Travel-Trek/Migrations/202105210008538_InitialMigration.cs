using System;

namespace Travel_Trek.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Posts",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    AgencyId = c.Int(nullable: false),
                    TripTitle = c.String(nullable: false, maxLength: 255),
                    TripDetails = c.String(maxLength: 255),
                    PostDate = c.DateTime(nullable: false, defaultValue: DateTime.Now),
                    TripDate = c.DateTime(nullable: false),
                    TripDestination = c.String(nullable: false, maxLength: 255),
                    TripImage = c.String(nullable: false),
                    Status = c.String(nullable: false, maxLength: 255, defaultValue: "Pending"),
                    Likes = c.Int(nullable: false, defaultValue: 0),
                    RefuseMessage = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.AgencyId, cascadeDelete: true)
                .Index(t => t.AgencyId);

            CreateTable(
                "dbo.People",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    FirstName = c.String(nullable: false, maxLength: 255),
                    LastName = c.String(maxLength: 255),
                    Email = c.String(maxLength: 255),
                    Password = c.String(nullable: false, maxLength: 255),
                    PhoneNumber = c.String(maxLength: 255),
                    Photo = c.String(maxLength: 255),
                    UserRoleId = c.Int(nullable: false)
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserRoles", t => t.UserRoleId, cascadeDelete: true)
                .Index(t => t.UserRoleId);

            CreateTable(
                "dbo.UserRoles",
                c => new
                {
                    Id = c.Int(nullable: false),
                    Name = c.String(nullable: false),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Posts", "AgencyId", "dbo.People");
            DropForeignKey("dbo.People", "UserRoleId", "dbo.UserRoles");
            DropIndex("dbo.People", new[] { "UserRoleId" });
            DropIndex("dbo.Posts", new[] { "AgencyId" });
            DropTable("dbo.UserRoles");
            DropTable("dbo.People");
            DropTable("dbo.Posts");
        }
    }
}
