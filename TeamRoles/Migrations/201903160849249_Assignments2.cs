namespace TeamRoles.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Assignments2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assignments", "AssignmentName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assignments", "AssignmentName");
        }
    }
}
