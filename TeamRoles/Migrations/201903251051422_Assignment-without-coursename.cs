namespace TeamRoles.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Assignmentwithoutcoursename : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Assignments", "CourseName");
            DropColumn("dbo.Posts", "UserName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posts", "UserName", c => c.String());
            AddColumn("dbo.Assignments", "CourseName", c => c.String());
        }
    }
}
