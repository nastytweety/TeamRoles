namespace TeamRoles.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationUserCourses", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserCourses", "Course_CourseId", "dbo.Courses");
            DropIndex("dbo.ApplicationUserCourses", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserCourses", new[] { "Course_CourseId" });
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        PostId = c.Int(nullable: false, identity: true),
                        PostText = c.String(),
                        PostDate = c.DateTime(nullable: false),
                        UserName = c.String(),
                        UserRole = c.String(),
                        ProfilePic = c.String(),
                    })
                .PrimaryKey(t => t.PostId);
            
            AddColumn("dbo.Courses", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Courses", "ApplicationUser_Id1", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "Course_CourseId", c => c.Int());
            AddColumn("dbo.AspNetUsers", "Post_PostId", c => c.Int());
            CreateIndex("dbo.Courses", "ApplicationUser_Id");
            CreateIndex("dbo.Courses", "ApplicationUser_Id1");
            CreateIndex("dbo.AspNetUsers", "Course_CourseId");
            CreateIndex("dbo.AspNetUsers", "Post_PostId");
            AddForeignKey("dbo.Courses", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Courses", "ApplicationUser_Id1", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "Course_CourseId", "dbo.Courses", "CourseId");
            AddForeignKey("dbo.AspNetUsers", "Post_PostId", "dbo.Posts", "PostId");
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
            
            DropForeignKey("dbo.AspNetUsers", "Post_PostId", "dbo.Posts");
            DropForeignKey("dbo.AspNetUsers", "Course_CourseId", "dbo.Courses");
            DropForeignKey("dbo.Courses", "ApplicationUser_Id1", "dbo.AspNetUsers");
            DropForeignKey("dbo.Courses", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", new[] { "Post_PostId" });
            DropIndex("dbo.AspNetUsers", new[] { "Course_CourseId" });
            DropIndex("dbo.Courses", new[] { "ApplicationUser_Id1" });
            DropIndex("dbo.Courses", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.AspNetUsers", "Post_PostId");
            DropColumn("dbo.AspNetUsers", "Course_CourseId");
            DropColumn("dbo.Courses", "ApplicationUser_Id1");
            DropColumn("dbo.Courses", "ApplicationUser_Id");
            DropTable("dbo.Posts");
            CreateIndex("dbo.ApplicationUserCourses", "Course_CourseId");
            CreateIndex("dbo.ApplicationUserCourses", "ApplicationUser_Id");
            AddForeignKey("dbo.ApplicationUserCourses", "Course_CourseId", "dbo.Courses", "CourseId", cascadeDelete: true);
            AddForeignKey("dbo.ApplicationUserCourses", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
