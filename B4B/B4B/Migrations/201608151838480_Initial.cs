namespace B4B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MedicalInfoProfiles", "MedicalInfo_MedicalInfoID", "dbo.MedicalInfoes");
            DropForeignKey("dbo.MedicalInfoProfiles", "Profile_ProfileID", "dbo.Profiles");
            DropForeignKey("dbo.PhotoProfiles", "Photo_PhotoID", "dbo.Photos");
            DropForeignKey("dbo.PhotoProfiles", "Profile_ProfileID", "dbo.Profiles");
            DropIndex("dbo.MedicalInfoProfiles", new[] { "MedicalInfo_MedicalInfoID" });
            DropIndex("dbo.MedicalInfoProfiles", new[] { "Profile_ProfileID" });
            DropIndex("dbo.PhotoProfiles", new[] { "Photo_PhotoID" });
            DropIndex("dbo.PhotoProfiles", new[] { "Profile_ProfileID" });
            AddColumn("dbo.AspNetUsers", "AdminName", c => c.String());
            DropTable("dbo.MedicalInfoes");
            DropTable("dbo.Photos");
            DropTable("dbo.MedicalInfoProfiles");
            DropTable("dbo.PhotoProfiles");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PhotoProfiles",
                c => new
                    {
                        Photo_PhotoID = c.Int(nullable: false),
                        Profile_ProfileID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Photo_PhotoID, t.Profile_ProfileID });
            
            CreateTable(
                "dbo.MedicalInfoProfiles",
                c => new
                    {
                        MedicalInfo_MedicalInfoID = c.Int(nullable: false),
                        Profile_ProfileID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MedicalInfo_MedicalInfoID, t.Profile_ProfileID });
            
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        PhotoID = c.Int(nullable: false, identity: true),
                        PhotoName = c.String(maxLength: 50),
                        PhotoBytes = c.Binary(),
                        Extension = c.String(),
                    })
                .PrimaryKey(t => t.PhotoID);
            
            CreateTable(
                "dbo.MedicalInfoes",
                c => new
                    {
                        MedicalInfoID = c.Int(nullable: false, identity: true),
                        MedicalInformation = c.String(),
                    })
                .PrimaryKey(t => t.MedicalInfoID);
            
            DropColumn("dbo.AspNetUsers", "AdminName");
            CreateIndex("dbo.PhotoProfiles", "Profile_ProfileID");
            CreateIndex("dbo.PhotoProfiles", "Photo_PhotoID");
            CreateIndex("dbo.MedicalInfoProfiles", "Profile_ProfileID");
            CreateIndex("dbo.MedicalInfoProfiles", "MedicalInfo_MedicalInfoID");
            AddForeignKey("dbo.PhotoProfiles", "Profile_ProfileID", "dbo.Profiles", "ProfileID", cascadeDelete: true);
            AddForeignKey("dbo.PhotoProfiles", "Photo_PhotoID", "dbo.Photos", "PhotoID", cascadeDelete: true);
            AddForeignKey("dbo.MedicalInfoProfiles", "Profile_ProfileID", "dbo.Profiles", "ProfileID", cascadeDelete: true);
            AddForeignKey("dbo.MedicalInfoProfiles", "MedicalInfo_MedicalInfoID", "dbo.MedicalInfoes", "MedicalInfoID", cascadeDelete: true);
        }
    }
}
