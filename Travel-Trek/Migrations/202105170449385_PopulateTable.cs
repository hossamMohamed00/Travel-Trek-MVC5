namespace Travel_Trek.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateTable : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO People(Id, FirstName, Email, Password, PhoneNumber, UserRoleId) VALUES (3, 'traveler', 'traveler@test.com', '12345', '0123456789', 3)");
            Sql("INSERT INTO People(Id, FirstName, Email, Password, PhoneNumber, UserRoleId) VALUES (4, 'ebram', 'ebram@test.com', '12345', '0123456789', 3)");

        }

        public override void Down()
        {
        }
    }
}
