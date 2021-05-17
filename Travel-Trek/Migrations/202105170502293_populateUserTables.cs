namespace Travel_Trek.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class populateUserTables : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO People(Id, FirstName, Email, Password, PhoneNumber, UserRoleId) VALUES (2, 'agency', 'agency@test.com', '12345', '0123456789', 2)");
            Sql("INSERT INTO People(Id, FirstName, Email, Password, PhoneNumber, UserRoleId) VALUES (3, 'traveler', 'traveler@test.com', '12345', '0123456789', 3)");
            Sql("INSERT INTO People(Id, FirstName, Email, Password, PhoneNumber, UserRoleId) VALUES (4, 'ebram', 'hossam@test.com', '12345', '0123456789', 1)");
            Sql("INSERT INTO People(Id, FirstName, Email, Password, PhoneNumber, UserRoleId) VALUES (5, 'hossam', 'ebram@test.com', '12345', '0123456789', 2)");
        }
        
        public override void Down()
        {
        }
    }
}
