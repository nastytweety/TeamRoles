namespace TeamRoles.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Postsfix : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Posts", "UserRole");
            DropColumn("dbo.Posts", "ProfilePic");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posts", "ProfilePic", c => c.String());
            AddColumn("dbo.Posts", "UserRole", c => c.String());
        }
    }
}
