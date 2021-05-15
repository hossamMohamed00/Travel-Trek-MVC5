namespace Travel_Trek.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPostStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "Status", c => c.String(maxLength: 255));
            AlterColumn("dbo.UserRoles", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserRoles", "Name", c => c.String());
            DropColumn("dbo.Posts", "Status");
        }
    }
}
