namespace B4B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedmanytomanyprofiletoemergencyContact : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EmergencyContacts", "ProfileID", "dbo.Profiles");
            DropIndex("dbo.EmergencyContacts", new[] { "ProfileID" });
            CreateTable(
                "dbo.ProfileEmergencyContacts",
                c => new
                    {
                        ProfileID = c.Int(nullable: false),
                        EmergencyContactID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProfileID, t.EmergencyContactID })
                .ForeignKey("dbo.Profiles", t => t.ProfileID, cascadeDelete: true)
                .ForeignKey("dbo.EmergencyContacts", t => t.EmergencyContactID, cascadeDelete: true)
                .Index(t => t.ProfileID)
                .Index(t => t.EmergencyContactID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProfileEmergencyContacts", "EmergencyContactID", "dbo.EmergencyContacts");
            DropForeignKey("dbo.ProfileEmergencyContacts", "ProfileID", "dbo.Profiles");
            DropIndex("dbo.ProfileEmergencyContacts", new[] { "EmergencyContactID" });
            DropIndex("dbo.ProfileEmergencyContacts", new[] { "ProfileID" });
            DropTable("dbo.ProfileEmergencyContacts");
            CreateIndex("dbo.EmergencyContacts", "ProfileID");
            AddForeignKey("dbo.EmergencyContacts", "ProfileID", "dbo.Profiles", "ProfileID", cascadeDelete: true);
        }
    }
}
