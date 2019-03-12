namespace TeamRoles.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Parents2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Children", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Child_ChildId", "dbo.Children");
            DropForeignKey("dbo.Children", "TheChild_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Children", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Children", new[] { "TheChild_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Child_ChildId" });
            DropPrimaryKey("dbo.Children");
            CreateTable(
                "dbo.ApplicationUserChilds",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Child_Childid = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Child_Childid })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Children", t => t.Child_Childid, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Child_Childid);
            
            AlterColumn("dbo.Children", "Childid", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Children", "Childid");
            DropColumn("dbo.Children", "ApplicationUser_Id");
            DropColumn("dbo.Children", "TheChild_Id");
            DropColumn("dbo.AspNetUsers", "Child_ChildId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Child_ChildId", c => c.Int());
            AddColumn("dbo.Children", "TheChild_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Children", "ApplicationUser_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.ApplicationUserChilds", "Child_Childid", "dbo.Children");
            DropForeignKey("dbo.ApplicationUserChilds", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationUserChilds", new[] { "Child_Childid" });
            DropIndex("dbo.ApplicationUserChilds", new[] { "ApplicationUser_Id" });
            DropPrimaryKey("dbo.Children");
            AlterColumn("dbo.Children", "Childid", c => c.Int(nullable: false, identity: true));
            DropTable("dbo.ApplicationUserChilds");
            AddPrimaryKey("dbo.Children", "ChildId");
            CreateIndex("dbo.AspNetUsers", "Child_ChildId");
            CreateIndex("dbo.Children", "TheChild_Id");
            CreateIndex("dbo.Children", "ApplicationUser_Id");
            AddForeignKey("dbo.Children", "TheChild_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "Child_ChildId", "dbo.Children", "ChildId");
            AddForeignKey("dbo.Children", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
