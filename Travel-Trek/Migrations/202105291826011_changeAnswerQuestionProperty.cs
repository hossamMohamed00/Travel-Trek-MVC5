namespace Travel_Trek.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeAnswerQuestionProperty : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserQuestions", "Answer", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserQuestions", "Answer", c => c.String(nullable: false));
        }
    }
}
