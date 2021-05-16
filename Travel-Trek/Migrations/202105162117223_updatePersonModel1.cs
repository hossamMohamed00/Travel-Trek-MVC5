namespace Travel_Trek.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatePersonModel1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.People", "UserRole_Id", "dbo.UserRoles");
            DropIndex("dbo.People", new[] { "UserRole_Id" });
            AlterColumn("dbo.People", "UserRole_Id", c => c.Byte());
            CreateIndex("dbo.People", "UserRole_Id");
            AddForeignKey("dbo.People", "UserRole_Id", "dbo.UserRoles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.People", "UserRole_Id", "dbo.UserRoles");
            DropIndex("dbo.People", new[] { "UserRole_Id" });
            AlterColumn("dbo.People", "UserRole_Id", c => c.Byte(nullable: false));
            CreateIndex("dbo.People", "UserRole_Id");
            AddForeignKey("dbo.People", "UserRole_Id", "dbo.UserRoles", "Id", cascadeDelete: true);
        }
    }
}
