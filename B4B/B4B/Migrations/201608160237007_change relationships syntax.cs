namespace B4B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changerelationshipssyntax : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmergencyContacts",
                c => new
                    {
                        EmergencyContactID = c.Int(nullable: false, identity: true),
                        EmergencyName = c.String(maxLength: 50),
                        EmergencyPhone = c.String(maxLength: 50),
                        ProfileID = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.EmergencyContactID)
                .ForeignKey("dbo.Profiles", t => t.ProfileID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.ProfileID)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.MedicalInfoes",
                c => new
                    {
                        MedicalInfoID = c.Int(nullable: false, identity: true),
                        MedicalInformation = c.String(),
                        ProfileID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MedicalInfoID)
                .ForeignKey("dbo.Profiles", t => t.ProfileID, cascadeDelete: true)
                .Index(t => t.ProfileID);
            
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        PhotoID = c.Int(nullable: false),
                        PhotoName = c.String(maxLength: 50),
                        PhotoBytes = c.Binary(),
                        Extension = c.String(),
                    })
                .PrimaryKey(t => t.PhotoID)
                .ForeignKey("dbo.Profiles", t => t.PhotoID)
                .Index(t => t.PhotoID);
            
            AddColumn("dbo.Profiles", "EmergencyContactID", c => c.Int(nullable: false));
            DropColumn("dbo.Profiles", "PhotoID");
            DropColumn("dbo.Profiles", "MedicalInfoID");
            DropColumn("dbo.Profiles", "EmergencyName");
            DropColumn("dbo.Profiles", "EmergencyPhone");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Profiles", "EmergencyPhone", c => c.String(maxLength: 50));
            AddColumn("dbo.Profiles", "EmergencyName", c => c.String(maxLength: 50));
            AddColumn("dbo.Profiles", "MedicalInfoID", c => c.Int(nullable: false));
            AddColumn("dbo.Profiles", "PhotoID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Photos", "PhotoID", "dbo.Profiles");
            DropForeignKey("dbo.MedicalInfoes", "ProfileID", "dbo.Profiles");
            DropForeignKey("dbo.EmergencyContacts", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.EmergencyContacts", "ProfileID", "dbo.Profiles");
            DropIndex("dbo.Photos", new[] { "PhotoID" });
            DropIndex("dbo.MedicalInfoes", new[] { "ProfileID" });
            DropIndex("dbo.EmergencyContacts", new[] { "User_Id" });
            DropIndex("dbo.EmergencyContacts", new[] { "ProfileID" });
            DropColumn("dbo.Profiles", "EmergencyContactID");
            DropTable("dbo.Photos");
            DropTable("dbo.MedicalInfoes");
            DropTable("dbo.EmergencyContacts");
        }
    }
}
