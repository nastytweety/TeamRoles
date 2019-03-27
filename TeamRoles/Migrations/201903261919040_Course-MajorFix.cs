namespace TeamRoles.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CourseMajorFix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationUserCourses", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserCourses", "Course_CourseId", "dbo.Courses");
            DropIndex("dbo.ApplicationUserCourses", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserCourses", new[] { "Course_CourseId" });
            AddColumn("dbo.Courses", "Teacher_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Courses", "Teacher_Id");
            AddForeignKey("dbo.Courses", "Teacher_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Courses", "TeacherName");
            DropColumn("dbo.Courses", "TeacherId");
            DropTable("dbo.ApplicationUserCourses");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ApplicationUserCourses",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Course_CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Course_CourseId });
            
            AddColumn("dbo.Courses", "TeacherId", c => c.String());
            AddColumn("dbo.Courses", "TeacherName", c => c.String());
            DropForeignKey("dbo.Courses", "Teacher_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Courses", new[] { "Teacher_Id" });
            DropColumn("dbo.Courses", "Teacher_Id");
            CreateIndex("dbo.ApplicationUserCourses", "Course_CourseId");
            CreateIndex("dbo.ApplicationUserCourses", "ApplicationUser_Id");
            AddForeignKey("dbo.ApplicationUserCourses", "Course_CourseId", "dbo.Courses", "CourseId", cascadeDelete: true);
            AddForeignKey("dbo.ApplicationUserCourses", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
