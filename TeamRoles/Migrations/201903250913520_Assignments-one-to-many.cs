namespace TeamRoles.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Assignmentsonetomany : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CourseAssignments", "Course_CourseId", "dbo.Courses");
            DropForeignKey("dbo.CourseAssignments", "Assignment_AssignmentId", "dbo.Assignments");
            DropIndex("dbo.CourseAssignments", new[] { "Course_CourseId" });
            DropIndex("dbo.CourseAssignments", new[] { "Assignment_AssignmentId" });
            AddColumn("dbo.Assignments", "Course_CourseId", c => c.Int());
            CreateIndex("dbo.Assignments", "Course_CourseId");
            AddForeignKey("dbo.Assignments", "Course_CourseId", "dbo.Courses", "CourseId");
            DropTable("dbo.CourseAssignments");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CourseAssignments",
                c => new
                    {
                        Course_CourseId = c.Int(nullable: false),
                        Assignment_AssignmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Course_CourseId, t.Assignment_AssignmentId });
            
            DropForeignKey("dbo.Assignments", "Course_CourseId", "dbo.Courses");
            DropIndex("dbo.Assignments", new[] { "Course_CourseId" });
            DropColumn("dbo.Assignments", "Course_CourseId");
            CreateIndex("dbo.CourseAssignments", "Assignment_AssignmentId");
            CreateIndex("dbo.CourseAssignments", "Course_CourseId");
            AddForeignKey("dbo.CourseAssignments", "Assignment_AssignmentId", "dbo.Assignments", "AssignmentId", cascadeDelete: true);
            AddForeignKey("dbo.CourseAssignments", "Course_CourseId", "dbo.Courses", "CourseId", cascadeDelete: true);
        }
    }
}
