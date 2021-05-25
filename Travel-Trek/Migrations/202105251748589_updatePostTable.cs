namespace Travel_Trek.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatePostTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Posts", "TripImage", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Posts", "TripImage", c => c.String(nullable: false));
        }
    }
}
