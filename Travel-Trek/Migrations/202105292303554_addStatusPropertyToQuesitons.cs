namespace Travel_Trek.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStatusPropertyToQuesitons : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserQuestions", "Status", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserQuestions", "Status");
        }
    }
}
