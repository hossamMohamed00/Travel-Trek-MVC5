namespace Travel_Trek.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatePeopleFields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.People", "FirstName", c => c.String(maxLength: 255));
            AlterColumn("dbo.People", "Password", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.People", "Password", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.People", "FirstName", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
 