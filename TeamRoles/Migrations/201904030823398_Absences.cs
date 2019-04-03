namespace TeamRoles.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Absences : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Enrollments", "Absences", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Enrollments", "Absences");
        }
    }
}
