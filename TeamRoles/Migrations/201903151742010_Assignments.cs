namespace TeamRoles.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Assignments : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ApplicationUserChilds", newName: "ChildApplicationUsers");
            RenameTable(name: "dbo.CourseApplicationUsers", newName: "ApplicationUserCourses");
            DropPrimaryKey("dbo.ChildApplicationUsers");
            DropPrimaryKey("dbo.ApplicationUserCourses");
            CreateTable(
                "dbo.Assignments",
                c => new
                    {
                        AssignmentId = c.Int(nullable: false, identity: true),
                        Filename = c.String(),
                        Path = c.String(),
                        DueDate = c.String(),
                        Points = c.Int(nullable: false),
                        CourseName = c.String(),
                        TeacherName = c.String(),
                    })
                .PrimaryKey(t => t.AssignmentId);
            
            CreateTable(
                "dbo.CourseAssignments",
                c => new
                    {
                        Course_CourseId = c.Int(nullable: false),
                        Assignment_AssignmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Course_CourseId, t.Assignment_AssignmentId })
                .ForeignKey("dbo.Courses", t => t.Course_CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Assignments", t => t.Assignment_AssignmentId, cascadeDelete: true)
                .Index(t => t.Course_CourseId)
                .Index(t => t.Assignment_AssignmentId);
            
            AddPrimaryKey("dbo.ChildApplicationUsers", new[] { "Child_Childid", "ApplicationUser_Id" });
            AddPrimaryKey("dbo.ApplicationUserCourses", new[] { "ApplicationUser_Id", "Course_CourseId" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CourseAssignments", "Assignment_AssignmentId", "dbo.Assignments");
            DropForeignKey("dbo.CourseAssignments", "Course_CourseId", "dbo.Courses");
            DropIndex("dbo.CourseAssignments", new[] { "Assignment_AssignmentId" });
            DropIndex("dbo.CourseAssignments", new[] { "Course_CourseId" });
            DropPrimaryKey("dbo.ApplicationUserCourses");
            DropPrimaryKey("dbo.ChildApplicationUsers");
            DropTable("dbo.CourseAssignments");
            DropTable("dbo.Assignments");
            AddPrimaryKey("dbo.ApplicationUserCourses", new[] { "Course_CourseId", "ApplicationUser_Id" });
            AddPrimaryKey("dbo.ChildApplicationUsers", new[] { "ApplicationUser_Id", "Child_Childid" });
            RenameTable(name: "dbo.ApplicationUserCourses", newName: "CourseApplicationUsers");
            RenameTable(name: "dbo.ChildApplicationUsers", newName: "ApplicationUserChilds");
        }
    }
}
