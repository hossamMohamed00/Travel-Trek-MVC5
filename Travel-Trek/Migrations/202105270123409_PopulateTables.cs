namespace Travel_Trek.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class PopulateTables : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO UserRoles (Name) VALUES ('Admin')");
            Sql("INSERT INTO UserRoles (Name) VALUES ('Agency')");
            Sql("INSERT INTO UserRoles (Name) VALUES ('Traveler')");

            Sql("INSERT INTO People(FirstName, Email, Password, PhoneNumber, UserRoleId) VALUES ('admin', 'admin@test.com', '12345', '0123456789', 1)");
            Sql("INSERT INTO People(FirstName, Email, Password, PhoneNumber, UserRoleId) VALUES ('agency', 'agency@test.com', '12345', '0123456789', 2)");
            Sql("INSERT INTO People(FirstName, Email, Password, PhoneNumber, UserRoleId) VALUES ('traveler', 'traveler@test.com', '12345', '0123456789', 3)");
        }

        public override void Down()
        {
        }
    }
}
