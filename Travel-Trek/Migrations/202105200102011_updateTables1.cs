namespace Travel_Trek.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTables1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Posts", "TripDetails", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Posts", "TripDetails", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
