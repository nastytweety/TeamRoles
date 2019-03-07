namespace TeamRoles.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Requests : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GenericRequests",
                c => new
                    {
                        ReqId = c.Int(nullable: false, identity: true),
                        Role = c.String(),
                        Type = c.String(),
                        Course_CourseId = c.Int(),
                        User1_Id = c.String(maxLength: 128),
                        User2_Id = c.String(maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ReqId)
                .ForeignKey("dbo.Courses", t => t.Course_CourseId)
                .ForeignKey("dbo.AspNetUsers", t => t.User1_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User2_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.Course_CourseId)
                .Index(t => t.User1_Id)
                .Index(t => t.User2_Id)
                .Index(t => t.ApplicationUser_Id);
            
            AddColumn("dbo.AspNetUsers", "GenericRequest_ReqId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "GenericRequest_ReqId");
            AddForeignKey("dbo.AspNetUsers", "GenericRequest_ReqId", "dbo.GenericRequests", "ReqId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GenericRequests", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.GenericRequests", "User2_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.GenericRequests", "User1_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.GenericRequests", "Course_CourseId", "dbo.Courses");
            DropForeignKey("dbo.AspNetUsers", "GenericRequest_ReqId", "dbo.GenericRequests");
            DropIndex("dbo.GenericRequests", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.GenericRequests", new[] { "User2_Id" });
            DropIndex("dbo.GenericRequests", new[] { "User1_Id" });
            DropIndex("dbo.GenericRequests", new[] { "Course_CourseId" });
            DropIndex("dbo.AspNetUsers", new[] { "GenericRequest_ReqId" });
            DropColumn("dbo.AspNetUsers", "GenericRequest_ReqId");
            DropTable("dbo.GenericRequests");
        }
    }
}
