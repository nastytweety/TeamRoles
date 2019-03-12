namespace TeamRoles.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Parent3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Child_ChildId", "dbo.Children");
            DropIndex("dbo.AspNetUsers", new[] { "Child_ChildId" });
            DropPrimaryKey("dbo.Children");
            AlterColumn("dbo.Children", "ChildId", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.AspNetUsers", "Child_ChildId", c => c.Int());
            AddPrimaryKey("dbo.Children", "ChildId");
            CreateIndex("dbo.AspNetUsers", "Child_ChildId");
            AddForeignKey("dbo.AspNetUsers", "Child_ChildId", "dbo.Children", "ChildId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Child_ChildId", "dbo.Children");
            DropIndex("dbo.AspNetUsers", new[] { "Child_ChildId" });
            DropPrimaryKey("dbo.Children");
            AlterColumn("dbo.AspNetUsers", "Child_ChildId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Children", "ChildId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Children", "ChildId");
            CreateIndex("dbo.AspNetUsers", "Child_ChildId");
            AddForeignKey("dbo.AspNetUsers", "Child_ChildId", "dbo.Children", "ChildId");
        }
    }
}
