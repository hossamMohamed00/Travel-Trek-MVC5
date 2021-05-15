namespace Travel_Trek.Migrations
{
      using System.Data.Entity.Migrations;

      public partial class populatePeopleTable : DbMigration
      {
            public override void Up()
            {
                  Sql("INSERT INTO People VALUES (1, 'admin', null, 'admin@test.com', '12345', '0123456789', null, 1)");
                  Sql("INSERT INTO People VALUES (2, 'agency', null, 'agency@test.com', '12345', '0123456789', null, 2)");
                  Sql("INSERT INTO People VALUES (3, 'traveler', null, 'traveler@test.com', '12345', '0123456789', null, 3)");
            }

            public override void Down()
            {
            }
      }
}
