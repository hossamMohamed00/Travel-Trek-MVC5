namespace Travel_Trek.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class populdatePerson : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO UserRoles (Id, Name) VALUES (1, 'Admin')");
            Sql("INSERT INTO UserRoles (Id, Name) VALUES (2, 'Agency')");
            Sql("INSERT INTO UserRoles (Id, Name) VALUES (3, 'Traveler')");

            Sql("INSERT INTO People(FirstName, Email, Password, PhoneNumber, UserRoleId) VALUES ('admin', 'admin@test.com', '12345', '0123456789', 1)");
            Sql("INSERT INTO People(FirstName, Email, Password, PhoneNumber, UserRoleId) VALUES ('agency', 'agency@test.com', '12345', '0123456789', 2)");
            Sql("INSERT INTO People(FirstName, Email, Password, PhoneNumber, UserRoleId) VALUES ('traveler', 'traveler@test.com', '12345', '0123456789', 3)");
        }

        public override void Down()
        {
        }
    }
}
