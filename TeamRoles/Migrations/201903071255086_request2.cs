namespace TeamRoles.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class request2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "GenericRequest_ReqId", "dbo.GenericRequests");
            DropForeignKey("dbo.GenericRequests", "Course_CourseId", "dbo.Courses");
            DropForeignKey("dbo.GenericRequests", "User1_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.GenericRequests", "User2_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.GenericRequests", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", new[] { "GenericRequest_ReqId" });
            DropIndex("dbo.GenericRequests", new[] { "Course_CourseId" });
            DropIndex("dbo.GenericRequests", new[] { "User1_Id" });
            DropIndex("dbo.GenericRequests", new[] { "User2_Id" });
            DropIndex("dbo.GenericRequests", new[] { "ApplicationUser_Id" });
            CreateTable(
                "dbo.GenericRequestApplicationUsers",
                c => new
                    {
                        GenericRequest_ReqId = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.GenericRequest_ReqId, t.ApplicationUser_Id })
                .ForeignKey("dbo.GenericRequests", t => t.GenericRequest_ReqId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.GenericRequest_ReqId)
                .Index(t => t.ApplicationUser_Id);
            
            AddColumn("dbo.GenericRequests", "User1id", c => c.String());
            AddColumn("dbo.GenericRequests", "User2id", c => c.String());
            AddColumn("dbo.GenericRequests", "Courseid", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "GenericRequest_ReqId");
            DropColumn("dbo.GenericRequests", "Course_CourseId");
            DropColumn("dbo.GenericRequests", "User1_Id");
            DropColumn("dbo.GenericRequests", "User2_Id");
            DropColumn("dbo.GenericRequests", "ApplicationUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GenericRequests", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.GenericRequests", "User2_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.GenericRequests", "User1_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.GenericRequests", "Course_CourseId", c => c.Int());
            AddColumn("dbo.AspNetUsers", "GenericRequest_ReqId", c => c.Int());
            DropForeignKey("dbo.GenericRequestApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.GenericRequestApplicationUsers", "GenericRequest_ReqId", "dbo.GenericRequests");
            DropIndex("dbo.GenericRequestApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.GenericRequestApplicationUsers", new[] { "GenericRequest_ReqId" });
            DropColumn("dbo.GenericRequests", "Courseid");
            DropColumn("dbo.GenericRequests", "User2id");
            DropColumn("dbo.GenericRequests", "User1id");
            DropTable("dbo.GenericRequestApplicationUsers");
            CreateIndex("dbo.GenericRequests", "ApplicationUser_Id");
            CreateIndex("dbo.GenericRequests", "User2_Id");
            CreateIndex("dbo.GenericRequests", "User1_Id");
            CreateIndex("dbo.GenericRequests", "Course_CourseId");
            CreateIndex("dbo.AspNetUsers", "GenericRequest_ReqId");
            AddForeignKey("dbo.GenericRequests", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.GenericRequests", "User2_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.GenericRequests", "User1_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.GenericRequests", "Course_CourseId", "dbo.Courses", "CourseId");
            AddForeignKey("dbo.AspNetUsers", "GenericRequest_ReqId", "dbo.GenericRequests", "ReqId");
        }
    }
}
