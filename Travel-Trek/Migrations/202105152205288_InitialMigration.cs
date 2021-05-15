namespace Travel_Trek.Migrations
{
    using System;
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
                        Status = c.String(maxLength: 255),
                        TripTitle = c.String(nullable: false, maxLength: 255),
                        TripDetails = c.String(nullable: false, maxLength: 255),
                        PostDate = c.DateTime(nullable: false),
                        TripDate = c.DateTime(nullable: false),
                        TripDestination = c.String(nullable: false, maxLength: 255),
                        TripImage = c.String(nullable: false),
                        Agency_Id = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.Agency_Id, cascadeDelete: true)
                .Index(t => t.Agency_Id);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        FirstName = c.String(nullable: false, maxLength: 255),
                        LastName = c.String(maxLength: 255),
                        Email = c.String(nullable: false, maxLength: 255),
                        Password = c.String(nullable: false, maxLength: 255),
                        PhoneNumber = c.String(maxLength: 255),
                        Photo = c.String(),
                        UserRole_Id = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserRoles", t => t.UserRole_Id, cascadeDelete: true)
                .Index(t => t.UserRole_Id);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "Agency_Id", "dbo.People");
            DropForeignKey("dbo.People", "UserRole_Id", "dbo.UserRoles");
            DropIndex("dbo.People", new[] { "UserRole_Id" });
            DropIndex("dbo.Posts", new[] { "Agency_Id" });
            DropTable("dbo.UserRoles");
            DropTable("dbo.People");
            DropTable("dbo.Posts");
        }
    }
}
