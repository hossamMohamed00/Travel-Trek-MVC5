namespace Travel_Trek.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddRefuseMessagePost : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "RefuseMessage", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Posts", "RefuseMessage");
        }
    }
}
