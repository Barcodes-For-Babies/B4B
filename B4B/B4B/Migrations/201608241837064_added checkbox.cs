namespace B4B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedcheckbox : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profiles", "disclaimer", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Profiles", "disclaimer");
        }
    }
}
