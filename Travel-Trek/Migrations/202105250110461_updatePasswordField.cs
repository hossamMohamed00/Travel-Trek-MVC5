namespace Travel_Trek.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatePasswordField : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.People", "Password", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.People", "Password", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
