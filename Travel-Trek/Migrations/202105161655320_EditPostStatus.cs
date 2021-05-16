namespace Travel_Trek.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditPostStatus : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Posts", "Status", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Posts", "Status", c => c.String(maxLength: 255));
        }
    }
}
