namespace TeamRoles.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Enrollment : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Grades", "GradeId", "dbo.Courses");
            DropIndex("dbo.Grades", new[] { "GradeId" });
            CreateTable(
                "dbo.Enrollments",
                c => new
                    {
                        EnrollmentId = c.Int(nullable: false, identity: true),
                        CourseId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        Grade = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.EnrollmentId)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.CourseId)
                .Index(t => t.UserId);
            
            DropTable("dbo.Grades");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Grades",
                c => new
                    {
                        GradeId = c.Int(nullable: false),
                        Type = c.String(),
                        NumericGrade = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.GradeId);
            
            DropForeignKey("dbo.Enrollments", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Enrollments", "CourseId", "dbo.Courses");
            DropIndex("dbo.Enrollments", new[] { "UserId" });
            DropIndex("dbo.Enrollments", new[] { "CourseId" });
            DropTable("dbo.Enrollments");
            CreateIndex("dbo.Grades", "GradeId");
            AddForeignKey("dbo.Grades", "GradeId", "dbo.Courses", "CourseId");
        }
    }
}
