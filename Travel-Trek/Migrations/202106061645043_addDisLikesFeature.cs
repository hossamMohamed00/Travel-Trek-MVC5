namespace Travel_Trek.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDisLikesFeature : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DisLikedPosts",
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
            
            AddColumn("dbo.Posts", "DisLikes", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DisLikedPosts", "PostId", "dbo.Posts");
            DropForeignKey("dbo.DisLikedPosts", "UserId", "dbo.People");
            DropIndex("dbo.DisLikedPosts", new[] { "PostId" });
            DropIndex("dbo.DisLikedPosts", new[] { "UserId" });
            DropColumn("dbo.Posts", "DisLikes");
            DropTable("dbo.DisLikedPosts");
        }
    }
}
