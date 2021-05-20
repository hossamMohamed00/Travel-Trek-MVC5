namespace Travel_Trek.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTables2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.People", "Email", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.People", "Email", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
