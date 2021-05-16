namespace Travel_Trek.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatePostModel1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Posts", new[] { "Agency_Id1" });
            DropColumn("dbo.Posts", "Agency_Id");
            RenameColumn(table: "dbo.Posts", name: "Agency_Id1", newName: "Agency_Id");
            AlterColumn("dbo.Posts", "Agency_Id", c => c.Byte());
            CreateIndex("dbo.Posts", "Agency_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Posts", new[] { "Agency_Id" });
            AlterColumn("dbo.Posts", "Agency_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Posts", name: "Agency_Id", newName: "Agency_Id1");
            AddColumn("dbo.Posts", "Agency_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Posts", "Agency_Id1");
        }
    }
}
