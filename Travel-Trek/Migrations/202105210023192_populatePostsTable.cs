namespace Travel_Trek.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class populatePostsTable : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Posts(TripTitle, TripDetails, TripDate, TripDestination, TripImage, AgencyId) VALUES ('Trip To Alex', 'Trip description', '1/1/2020 12:00:00 AM', 'Alex', '/Content/images/posts/trip_1.jpg', 2)");
            Sql("INSERT INTO Posts(TripTitle, TripDetails, TripDate, TripDestination, TripImage, AgencyId) VALUES ('Trip To Alex', 'Trip description', '1/1/2020 12:00:00 AM', 'Alex', '/Content/images/posts/trip_1.jpg', 2)");
            Sql("INSERT INTO Posts(TripTitle, TripDetails, TripDate, TripDestination, TripImage, AgencyId) VALUES ('Trip To Alex', 'Trip description', '1/1/2020 12:00:00 AM', 'Alex', '/Content/images/posts/trip_1.jpg', 2)");
            Sql("INSERT INTO Posts(TripTitle, TripDetails, TripDate, TripDestination, TripImage, AgencyId) VALUES ('Trip To Alex', 'Trip description', '1/1/2020 12:00:00 AM', 'Alex', '/Content/images/posts/trip_1.jpg', 2)");
        }

        public override void Down()
        {
        }
    }
}
