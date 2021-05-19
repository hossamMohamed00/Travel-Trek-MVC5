namespace Travel_Trek.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class populateTables : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO UserRoles (Id, Name) VALUES (1, 'Admin')");
            Sql("INSERT INTO UserRoles (Id, Name) VALUES (2, 'Agency')");
            Sql("INSERT INTO UserRoles (Id, Name) VALUES (3, 'Traveler')");

            Sql("INSERT INTO People(FirstName, Email, Password, PhoneNumber, UserRoleId) VALUES ('admin', 'admin@test.com', '123', '0123456789', 1)");
            Sql("INSERT INTO People(FirstName, Email, Password, PhoneNumber, UserRoleId) VALUES ('agency', 'agency@test.com', '123', '0123456789', 2)");
            Sql("INSERT INTO People(FirstName, Email, Password, PhoneNumber, UserRoleId) VALUES ('traveler', 'traveler@test.com', '123', '0123456789', 3)");


            Sql("INSERT INTO Posts(Status, TripTitle, TripDetails, PostDate, TripDate, TripDestination, TripImage, Likes, AgencyId) VALUES ('Pending', 'Trip To Alex', 'Trip description', '1 Jan 2020', '5 Jan 2020', 'Alex', 'Trip Image Here', 0, 2)");
            Sql("INSERT INTO Posts(Status, TripTitle, TripDetails, PostDate, TripDate, TripDestination, TripImage, Likes, AgencyId) VALUES ('Approved', 'Trip To Paris', 'Trip description', '1 Dec 2021', '5 Dec 2021', 'Paris', 'Trip Image Here', 0, 2)");
            Sql("INSERT INTO Posts(Status, TripTitle, TripDetails, PostDate, TripDate, TripDestination, TripImage, Likes, AgencyId) VALUES ('Pending', 'Trip To Alex', 'Trip description', '1 Jan 2020', '5 Jan 2020', 'Alex', 'Trip Image Here', 0, 2)");
            Sql("INSERT INTO Posts(Status, TripTitle, TripDetails, PostDate, TripDate, TripDestination, TripImage, Likes, AgencyId) VALUES ('Pending', 'Trip To Paris', 'Trip description', '1 Dec 2021', '5 Dec 2021', 'Paris', 'Trip Image Here', 0, 2)");

        }

        public override void Down()
        {
        }
    }
}
