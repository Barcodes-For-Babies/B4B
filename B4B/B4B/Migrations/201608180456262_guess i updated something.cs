namespace B4B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class guessiupdatedsomething : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.EmergencyContactProfiles", newName: "ProfileEmergencyContacts");
            DropPrimaryKey("dbo.ProfileEmergencyContacts");
            AddPrimaryKey("dbo.ProfileEmergencyContacts", new[] { "Profile_ProfileID", "EmergencyContact_EmergencyContactID" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.ProfileEmergencyContacts");
            AddPrimaryKey("dbo.ProfileEmergencyContacts", new[] { "EmergencyContact_EmergencyContactID", "Profile_ProfileID" });
            RenameTable(name: "dbo.ProfileEmergencyContacts", newName: "EmergencyContactProfiles");
        }
    }
}
