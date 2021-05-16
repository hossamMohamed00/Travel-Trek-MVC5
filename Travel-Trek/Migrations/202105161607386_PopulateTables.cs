namespace Travel_Trek.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class PopulateTables : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO UserRoles (Id, Name) VALUES (1, 'Admin')");
            Sql("INSERT INTO UserRoles (Id, Name) VALUES (2, 'Agency')");
            Sql("INSERT INTO UserRoles (Id, Name) VALUES (3, 'Traveler')");

            Sql("INSERT INTO People(Id, FirstName, Email, Password, PhoneNumber, UserRole_Id) VALUES (1, 'admin', 'admin@test.com', '12345', '0123456789', 1)");
            Sql("INSERT INTO People(Id, FirstName, Email, Password, PhoneNumber, UserRole_Id) VALUES (2, 'agency', 'agency@test.com', '12345', '0123456789', 2)");
            Sql("INSERT INTO People(Id, FirstName, Email, Password, PhoneNumber, UserRole_Id) VALUES (3, 'traveler', 'traveler@test.com', '12345', '0123456789', 3)");

            Sql("INSERT INTO Posts(Status, TripTitle, TripDetails, PostDate, TripDate, TripDestination, TripImage, Agency_Id) VALUES ('Pending', 'Trip To Alex', 'Trip description', '1 Jan 2020', '5 Jan 2020', 'Alex', 'Trip Image Here', 2)");
            Sql("INSERT INTO Posts(Status, TripTitle, TripDetails, PostDate, TripDate, TripDestination, TripImage, Agency_Id) VALUES ('Approved', 'Trip To Paris', 'Trip description', '1 Dec 2021', '5 Dec 2021', 'Paris', 'Trip Image Here', 2)");
        }

        public override void Down()
        {
        }
    }
}
