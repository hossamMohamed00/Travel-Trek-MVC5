namespace Travel_Trek.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addUsersTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.People", "UserRole_Id", "dbo.UserRoles");
            DropIndex("dbo.People", new[] { "UserRole_Id" });
            AddColumn("dbo.UserRoles", "Name", c => c.String());
            AlterColumn("dbo.People", "UserRole_Id", c => c.Byte(nullable: false));
            CreateIndex("dbo.People", "UserRole_Id");
            AddForeignKey("dbo.People", "UserRole_Id", "dbo.UserRoles", "Id", cascadeDelete: true);
            DropColumn("dbo.People", "UserRoleId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.People", "UserRoleId", c => c.Int(nullable: false));
            DropForeignKey("dbo.People", "UserRole_Id", "dbo.UserRoles");
            DropIndex("dbo.People", new[] { "UserRole_Id" });
            AlterColumn("dbo.People", "UserRole_Id", c => c.Byte());
            DropColumn("dbo.UserRoles", "Name");
            CreateIndex("dbo.People", "UserRole_Id");
            AddForeignKey("dbo.People", "UserRole_Id", "dbo.UserRoles", "Id");
        }
    }
}
