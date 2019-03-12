namespace TeamRoles.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Parents : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationUserChilds", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserChilds", "Child_ChildId", "dbo.Children");
            DropIndex("dbo.ApplicationUserChilds", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserChilds", new[] { "Child_ChildId" });
            AddColumn("dbo.Children", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Children", "TheChild_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "Child_ChildId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Children", "ApplicationUser_Id");
            CreateIndex("dbo.Children", "TheChild_Id");
            CreateIndex("dbo.AspNetUsers", "Child_ChildId");
            AddForeignKey("dbo.Children", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "Child_ChildId", "dbo.Children", "ChildId");
            AddForeignKey("dbo.Children", "TheChild_Id", "dbo.AspNetUsers", "Id");
            DropTable("dbo.ApplicationUserChilds");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ApplicationUserChilds",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Child_ChildId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Child_ChildId });
            
            DropForeignKey("dbo.Children", "TheChild_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Child_ChildId", "dbo.Children");
            DropForeignKey("dbo.Children", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", new[] { "Child_ChildId" });
            DropIndex("dbo.Children", new[] { "TheChild_Id" });
            DropIndex("dbo.Children", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.AspNetUsers", "Child_ChildId");
            DropColumn("dbo.Children", "TheChild_Id");
            DropColumn("dbo.Children", "ApplicationUser_Id");
            CreateIndex("dbo.ApplicationUserChilds", "Child_ChildId");
            CreateIndex("dbo.ApplicationUserChilds", "ApplicationUser_Id");
            AddForeignKey("dbo.ApplicationUserChilds", "Child_ChildId", "dbo.Children", "ChildId", cascadeDelete: true);
            AddForeignKey("dbo.ApplicationUserChilds", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
