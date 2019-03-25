namespace TeamRoles.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Requestsonetomany : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GenericRequestApplicationUsers", "GenericRequest_ReqId", "dbo.GenericRequests");
            DropForeignKey("dbo.GenericRequestApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.GenericRequestApplicationUsers", new[] { "GenericRequest_ReqId" });
            DropIndex("dbo.GenericRequestApplicationUsers", new[] { "ApplicationUser_Id" });
            AddColumn("dbo.GenericRequests", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.GenericRequests", "ApplicationUser_Id");
            AddForeignKey("dbo.GenericRequests", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            DropTable("dbo.GenericRequestApplicationUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.GenericRequestApplicationUsers",
                c => new
                    {
                        GenericRequest_ReqId = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.GenericRequest_ReqId, t.ApplicationUser_Id });
            
            DropForeignKey("dbo.GenericRequests", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.GenericRequests", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.GenericRequests", "ApplicationUser_Id");
            CreateIndex("dbo.GenericRequestApplicationUsers", "ApplicationUser_Id");
            CreateIndex("dbo.GenericRequestApplicationUsers", "GenericRequest_ReqId");
            AddForeignKey("dbo.GenericRequestApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.GenericRequestApplicationUsers", "GenericRequest_ReqId", "dbo.GenericRequests", "ReqId", cascadeDelete: true);
        }
    }
}
