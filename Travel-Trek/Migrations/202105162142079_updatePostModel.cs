namespace Travel_Trek.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatePostModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Posts", "Agency_Id", "dbo.People");
            DropIndex("dbo.Posts", new[] { "Agency_Id" });
            AddColumn("dbo.Posts", "Agency_Id1", c => c.Byte());
            AlterColumn("dbo.Posts", "Agency_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Posts", "Agency_Id1");
            AddForeignKey("dbo.Posts", "Agency_Id1", "dbo.People", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "Agency_Id1", "dbo.People");
            DropIndex("dbo.Posts", new[] { "Agency_Id1" });
            AlterColumn("dbo.Posts", "Agency_Id", c => c.Byte(nullable: false));
            DropColumn("dbo.Posts", "Agency_Id1");
            CreateIndex("dbo.Posts", "Agency_Id");
            AddForeignKey("dbo.Posts", "Agency_Id", "dbo.People", "Id", cascadeDelete: true);
        }
    }
}
