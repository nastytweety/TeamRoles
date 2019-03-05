namespace TeamRoles.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CourseFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "CourseDescription", c => c.String());
            AddColumn("dbo.Courses", "CoursePic", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "CoursePic");
            DropColumn("dbo.Courses", "CourseDescription");
        }
    }
}
