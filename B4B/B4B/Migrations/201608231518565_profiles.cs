namespace B4B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class profiles : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Profiles", "FirstName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Profiles", "EcontactName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Profiles", "EmergencyPhone", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Profiles", "EmergencyPhone", c => c.String());
            AlterColumn("dbo.Profiles", "EcontactName", c => c.String(maxLength: 100));
            AlterColumn("dbo.Profiles", "FirstName", c => c.String(maxLength: 50));
        }
    }
}
