namespace Travel_Trek.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPriceToPost : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "Price", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "Price");
        }
    }
}
