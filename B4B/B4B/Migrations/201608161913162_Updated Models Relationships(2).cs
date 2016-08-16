namespace B4B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedModelsRelationships2 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ProfileEmergencyContacts", newName: "EmergencyContactProfiles");
            RenameColumn(table: "dbo.EmergencyContactProfiles", name: "ProfileID", newName: "Profile_ProfileID");
            RenameColumn(table: "dbo.EmergencyContactProfiles", name: "EmergencyContactID", newName: "EmergencyContact_EmergencyContactID");
            RenameIndex(table: "dbo.EmergencyContactProfiles", name: "IX_EmergencyContactID", newName: "IX_EmergencyContact_EmergencyContactID");
            RenameIndex(table: "dbo.EmergencyContactProfiles", name: "IX_ProfileID", newName: "IX_Profile_ProfileID");
            DropPrimaryKey("dbo.EmergencyContactProfiles");
            AddPrimaryKey("dbo.EmergencyContactProfiles", new[] { "EmergencyContact_EmergencyContactID", "Profile_ProfileID" });
            DropColumn("dbo.Profiles", "EmergencyContactID");
            DropColumn("dbo.EmergencyContacts", "ProfileID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EmergencyContacts", "ProfileID", c => c.Int(nullable: false));
            AddColumn("dbo.Profiles", "EmergencyContactID", c => c.Int(nullable: false));
            DropPrimaryKey("dbo.EmergencyContactProfiles");
            AddPrimaryKey("dbo.EmergencyContactProfiles", new[] { "ProfileID", "EmergencyContactID" });
            RenameIndex(table: "dbo.EmergencyContactProfiles", name: "IX_Profile_ProfileID", newName: "IX_ProfileID");
            RenameIndex(table: "dbo.EmergencyContactProfiles", name: "IX_EmergencyContact_EmergencyContactID", newName: "IX_EmergencyContactID");
            RenameColumn(table: "dbo.EmergencyContactProfiles", name: "EmergencyContact_EmergencyContactID", newName: "EmergencyContactID");
            RenameColumn(table: "dbo.EmergencyContactProfiles", name: "Profile_ProfileID", newName: "ProfileID");
            RenameTable(name: "dbo.EmergencyContactProfiles", newName: "ProfileEmergencyContacts");
        }
    }
}
