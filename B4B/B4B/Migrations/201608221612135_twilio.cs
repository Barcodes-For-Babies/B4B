namespace B4B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class twilio : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Profiles", "EmergencyPhone", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Profiles", "EmergencyPhone", c => c.String(nullable: false));
        }
    }
}
