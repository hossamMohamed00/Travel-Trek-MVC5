namespace Travel_Trek.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addQuestionDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserQuestions", "Date", c => c.DateTime(nullable: false, defaultValue: DateTime.Now));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserQuestions", "Date");
        }
    }
}
