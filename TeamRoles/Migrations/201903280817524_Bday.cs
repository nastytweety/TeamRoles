namespace TeamRoles.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Bday : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "BirthDay", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "BirthDay");
        }
    }
}
