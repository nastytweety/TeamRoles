namespace TeamRoles.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Grades : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Grades",
                c => new
                    {
                        GradeId = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        NumericGrade = c.Double(nullable: false),
                        Course_CourseId = c.Int(),
                    })
                .PrimaryKey(t => t.GradeId)
                .ForeignKey("dbo.Courses", t => t.Course_CourseId)
                .Index(t => t.Course_CourseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Grades", "Course_CourseId", "dbo.Courses");
            DropIndex("dbo.Grades", new[] { "Course_CourseId" });
            DropTable("dbo.Grades");
        }
    }
}
