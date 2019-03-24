namespace TeamRoles.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mergegeorge : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Courses", "CoursePic", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Courses", "CoursePic", c => c.String(nullable: false));
        }
    }
}
