namespace B4B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateProfileWithEmergencyContact : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EmergencyContacts", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProfileEmergencyContacts", "Profile_ProfileID", "dbo.Profiles");
            DropForeignKey("dbo.ProfileEmergencyContacts", "EmergencyContact_EmergencyContactID", "dbo.EmergencyContacts");
            DropIndex("dbo.EmergencyContacts", new[] { "User_Id" });
            DropIndex("dbo.ProfileEmergencyContacts", new[] { "Profile_ProfileID" });
            DropIndex("dbo.ProfileEmergencyContacts", new[] { "EmergencyContact_EmergencyContactID" });
            AddColumn("dbo.Profiles", "EcontactFirstName", c => c.String(maxLength: 50));
            AddColumn("dbo.Profiles", "EcontactLasttName", c => c.String(maxLength: 50));
            AddColumn("dbo.Profiles", "EmergencyPhone", c => c.String(maxLength: 50));
            DropTable("dbo.ProfileEmergencyContacts");
            DropTable("dbo.EmergencyContacts");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProfileEmergencyContacts",
                c => new
                    {
                        Profile_ProfileID = c.Int(nullable: false),
                        EmergencyContact_EmergencyContactID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Profile_ProfileID, t.EmergencyContact_EmergencyContactID });
            
            CreateTable(
                "dbo.EmergencyContacts",
                c => new
                    {
                        EmergencyContactID = c.Int(nullable: false, identity: true),
                        EmergencyName = c.String(maxLength: 50),
                        EmergencyPhone = c.String(maxLength: 50),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.EmergencyContactID);
            
            DropColumn("dbo.Profiles", "EmergencyPhone");
            DropColumn("dbo.Profiles", "EcontactLasttName");
            DropColumn("dbo.Profiles", "EcontactFirstName");
            CreateIndex("dbo.ProfileEmergencyContacts", "EmergencyContact_EmergencyContactID");
            CreateIndex("dbo.ProfileEmergencyContacts", "Profile_ProfileID");
            CreateIndex("dbo.EmergencyContacts", "User_Id");
            AddForeignKey("dbo.ProfileEmergencyContacts", "EmergencyContact_EmergencyContactID", "dbo.EmergencyContacts", "EmergencyContactID", cascadeDelete: true);
            AddForeignKey("dbo.ProfileEmergencyContacts", "Profile_ProfileID", "dbo.Profiles", "ProfileID", cascadeDelete: true);
            AddForeignKey("dbo.EmergencyContacts", "User_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
