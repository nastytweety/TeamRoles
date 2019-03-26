namespace TeamRoles.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Assignmentwithoutteachername : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Assignments", "TeacherName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Assignments", "TeacherName", c => c.String());
        }
    }
}
