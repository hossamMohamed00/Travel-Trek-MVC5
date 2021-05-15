namespace Travel_Trek.Migrations
{
      using System.Data.Entity.Migrations;

      public partial class addPostsTable1 : DbMigration
      {
            public override void Up()
            {
                  CreateTable(
                      "dbo.Posts",
                      c => new
                      {
                            Id = c.Int(nullable: false, identity: true),
                            TripTitle = c.String(nullable: false, maxLength: 255),
                            TripDetails = c.String(nullable: false, maxLength: 255),
                            PostDate = c.DateTime(nullable: false),
                            TripDate = c.DateTime(nullable: false),
                            TripDestination = c.String(nullable: false, maxLength: 255),
                            TripImage = c.String(nullable: false),
                            AgencyName_Id = c.Byte(nullable: false),
                      })
                      .PrimaryKey(t => t.Id)
                      .ForeignKey("dbo.People", t => t.AgencyName_Id, cascadeDelete: true)
                      .Index(t => t.AgencyName_Id);

            }

            public override void Down()
            {
                  DropForeignKey("dbo.Posts", "AgencyName_Id", "dbo.People");
                  DropIndex("dbo.Posts", new[] { "AgencyName_Id" });
                  DropTable("dbo.Posts");
            }
      }
}
