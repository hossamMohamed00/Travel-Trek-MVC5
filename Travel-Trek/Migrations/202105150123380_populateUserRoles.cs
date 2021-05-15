namespace Travel_Trek.Migrations
{
      using System.Data.Entity.Migrations;

      public partial class populateUserRoles : DbMigration
      {
            public override void Up()
            {
                  Sql("INSERT INTO UserRoles (Id, Name) VALUES (1, 'Admin')");
                  Sql("INSERT INTO UserRoles (Id, Name) VALUES (2, 'Agency')");
                  Sql("INSERT INTO UserRoles (Id, Name) VALUES (3, 'Traveler')");
            }

            public override void Down()
            {
            }
      }
}
