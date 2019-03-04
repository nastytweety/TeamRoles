namespace TeamRoles.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Courses", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Courses", "ApplicationUser_Id1", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Course_CourseId", "dbo.Courses");
            DropForeignKey("dbo.AspNetUsers", "Post_PostId", "dbo.Posts");
            DropIndex("dbo.Courses", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Courses", new[] { "ApplicationUser_Id1" });
            DropIndex("dbo.AspNetUsers", new[] { "Course_CourseId" });
            DropIndex("dbo.AspNetUsers", new[] { "Post_PostId" });
            CreateTable(
                "dbo.ApplicationUserCourses",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Course_CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Course_CourseId })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Courses", t => t.Course_CourseId, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Course_CourseId);
            
            CreateTable(
                "dbo.PostApplicationUsers",
                c => new
                    {
                        Post_PostId = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Post_PostId, t.ApplicationUser_Id })
                .ForeignKey("dbo.Posts", t => t.Post_PostId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.Post_PostId)
                .Index(t => t.ApplicationUser_Id);
            
            DropColumn("dbo.Courses", "ApplicationUser_Id");
            DropColumn("dbo.Courses", "ApplicationUser_Id1");
            DropColumn("dbo.AspNetUsers", "Course_CourseId");
            DropColumn("dbo.AspNetUsers", "Post_PostId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Post_PostId", c => c.Int());
            AddColumn("dbo.AspNetUsers", "Course_CourseId", c => c.Int());
            AddColumn("dbo.Courses", "ApplicationUser_Id1", c => c.String(maxLength: 128));
            AddColumn("dbo.Courses", "ApplicationUser_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.PostApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PostApplicationUsers", "Post_PostId", "dbo.Posts");
            DropForeignKey("dbo.ApplicationUserCourses", "Course_CourseId", "dbo.Courses");
            DropForeignKey("dbo.ApplicationUserCourses", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.PostApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.PostApplicationUsers", new[] { "Post_PostId" });
            DropIndex("dbo.ApplicationUserCourses", new[] { "Course_CourseId" });
            DropIndex("dbo.ApplicationUserCourses", new[] { "ApplicationUser_Id" });
            DropTable("dbo.PostApplicationUsers");
            DropTable("dbo.ApplicationUserCourses");
            CreateIndex("dbo.AspNetUsers", "Post_PostId");
            CreateIndex("dbo.AspNetUsers", "Course_CourseId");
            CreateIndex("dbo.Courses", "ApplicationUser_Id1");
            CreateIndex("dbo.Courses", "ApplicationUser_Id");
            AddForeignKey("dbo.AspNetUsers", "Post_PostId", "dbo.Posts", "PostId");
            AddForeignKey("dbo.AspNetUsers", "Course_CourseId", "dbo.Courses", "CourseId");
            AddForeignKey("dbo.Courses", "ApplicationUser_Id1", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Courses", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
