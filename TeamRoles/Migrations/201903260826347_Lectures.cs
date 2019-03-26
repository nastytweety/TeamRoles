namespace TeamRoles.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Lectures : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Lectures",
                c => new
                    {
                        LectureId = c.Int(nullable: false, identity: true),
                        LectureName = c.String(),
                        Filename = c.String(),
                        Path = c.String(),
                        PostDate = c.DateTime(nullable: false),
                        Course_CourseId = c.Int(),
                    })
                .PrimaryKey(t => t.LectureId)
                .ForeignKey("dbo.Courses", t => t.Course_CourseId)
                .Index(t => t.Course_CourseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Lectures", "Course_CourseId", "dbo.Courses");
            DropIndex("dbo.Lectures", new[] { "Course_CourseId" });
            DropTable("dbo.Lectures");
        }
    }
}
