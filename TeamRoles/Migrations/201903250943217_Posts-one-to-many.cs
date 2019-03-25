namespace TeamRoles.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Postsonetomany : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PostApplicationUsers", "Post_PostId", "dbo.Posts");
            DropForeignKey("dbo.PostApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.PostApplicationUsers", new[] { "Post_PostId" });
            DropIndex("dbo.PostApplicationUsers", new[] { "ApplicationUser_Id" });
            AddColumn("dbo.Posts", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Posts", "ApplicationUser_Id");
            AddForeignKey("dbo.Posts", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            DropTable("dbo.PostApplicationUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PostApplicationUsers",
                c => new
                    {
                        Post_PostId = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Post_PostId, t.ApplicationUser_Id });
            
            DropForeignKey("dbo.Posts", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Posts", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Posts", "ApplicationUser_Id");
            CreateIndex("dbo.PostApplicationUsers", "ApplicationUser_Id");
            CreateIndex("dbo.PostApplicationUsers", "Post_PostId");
            AddForeignKey("dbo.PostApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PostApplicationUsers", "Post_PostId", "dbo.Posts", "PostId", cascadeDelete: true);
        }
    }
}
