namespace Travel_Trek.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addUserQuestionsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserQuestions",
                c => new
                {
                    UserId = c.Int(nullable: false),
                    PostId = c.Int(nullable: false),
                    Question = c.String(nullable: false),
                    Answer = c.String(nullable: false),
                })
                .PrimaryKey(t => new { t.UserId, t.PostId })
                .ForeignKey("dbo.Posts", t => t.PostId)
                .ForeignKey("dbo.People", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.PostId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.UserQuestions", "UserId", "dbo.People");
            DropForeignKey("dbo.UserQuestions", "PostId", "dbo.Posts");
            DropIndex("dbo.UserQuestions", new[] { "PostId" });
            DropIndex("dbo.UserQuestions", new[] { "UserId" });
            DropTable("dbo.UserQuestions");
        }
    }
}
