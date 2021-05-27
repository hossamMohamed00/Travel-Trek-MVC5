namespace Travel_Trek.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addLikedPosts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LikedPosts",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        PostId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.PostId })
                .ForeignKey("dbo.People", t => t.UserId)
                .ForeignKey("dbo.Posts", t => t.PostId)
                .Index(t => t.UserId)
                .Index(t => t.PostId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LikedPosts", "PostId", "dbo.Posts");
            DropForeignKey("dbo.LikedPosts", "UserId", "dbo.People");
            DropIndex("dbo.LikedPosts", new[] { "PostId" });
            DropIndex("dbo.LikedPosts", new[] { "UserId" });
            DropTable("dbo.LikedPosts");
        }
    }
}
