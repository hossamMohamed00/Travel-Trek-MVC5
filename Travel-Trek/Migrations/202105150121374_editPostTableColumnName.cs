namespace Travel_Trek.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editPostTableColumnName : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Posts", name: "AgencyName_Id", newName: "Agency_Id");
            RenameIndex(table: "dbo.Posts", name: "IX_AgencyName_Id", newName: "IX_Agency_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Posts", name: "IX_Agency_Id", newName: "IX_AgencyName_Id");
            RenameColumn(table: "dbo.Posts", name: "Agency_Id", newName: "AgencyName_Id");
        }
    }
}
