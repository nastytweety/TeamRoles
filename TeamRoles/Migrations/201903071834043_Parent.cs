namespace TeamRoles.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Parent : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ApplicationUserCourses", newName: "CourseApplicationUsers");
            DropPrimaryKey("dbo.CourseApplicationUsers");
            CreateTable(
                "dbo.Children",
                c => new
                    {
                        ChildId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ChildId);
            
            CreateTable(
                "dbo.ApplicationUserChilds",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Child_ChildId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Child_ChildId })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Children", t => t.Child_ChildId, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Child_ChildId);
            
            AddPrimaryKey("dbo.CourseApplicationUsers", new[] { "Course_CourseId", "ApplicationUser_Id" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationUserChilds", "Child_ChildId", "dbo.Children");
            DropForeignKey("dbo.ApplicationUserChilds", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationUserChilds", new[] { "Child_ChildId" });
            DropIndex("dbo.ApplicationUserChilds", new[] { "ApplicationUser_Id" });
            DropPrimaryKey("dbo.CourseApplicationUsers");
            DropTable("dbo.ApplicationUserChilds");
            DropTable("dbo.Children");
            AddPrimaryKey("dbo.CourseApplicationUsers", new[] { "ApplicationUser_Id", "Course_CourseId" });
            RenameTable(name: "dbo.CourseApplicationUsers", newName: "ApplicationUserCourses");
        }
    }
}
