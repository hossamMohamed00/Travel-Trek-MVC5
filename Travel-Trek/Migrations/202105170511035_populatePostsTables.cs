namespace Travel_Trek.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class populatePostsTables : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Posts(Status, TripTitle, TripDetails, PostDate, TripDate, TripDestination, TripImage, Likes, AgencyId) VALUES ('Pending', 'Trip To Alex', 'Trip description', '1 Jan 2020', '5 Jan 2020', 'Alex', 'Trip Image Here', 0, 2)");
            Sql("INSERT INTO Posts(Status, TripTitle, TripDetails, PostDate, TripDate, TripDestination, TripImage, Likes, AgencyId) VALUES ('Approved', 'Trip To Paris', 'Trip description', '1 Dec 2021', '5 Dec 2021', 'Paris', 'Trip Image Here', 0, 2)");
        }
        
        public override void Down()
        {
        }
    }
}
