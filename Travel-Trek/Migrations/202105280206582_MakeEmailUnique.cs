namespace Travel_Trek.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeEmailUnique : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.People", "Email", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.People", new[] { "Email" });
        }
    }
}
