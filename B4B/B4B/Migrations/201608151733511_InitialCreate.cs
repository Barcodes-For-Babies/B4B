namespace B4B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Profiles",
                c => new
                    {
                        ProfileID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(maxLength: 50),
                        LastName = c.String(maxLength: 50),
                        PhotoID = c.Int(nullable: false),
                        StreetAdress = c.String(maxLength: 50),
                        City = c.String(maxLength: 50),
                        State = c.String(maxLength: 50),
                        ZipCode = c.Int(nullable: false),
                        MedicalInfoID = c.Int(nullable: false),
                        EmergencyName = c.String(maxLength: 50),
                        EmergencyPhone = c.String(maxLength: 50),
                        Admin_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ProfileID)
                .ForeignKey("dbo.AspNetUsers", t => t.Admin_Id)
                .Index(t => t.Admin_Id);
            
            CreateTable(
                "dbo.MedicalInfoes",
                c => new
                    {
                        MedicalInfoID = c.Int(nullable: false, identity: true),
                        MedicalInformation = c.String(),
                    })
                .PrimaryKey(t => t.MedicalInfoID);
            
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
                "dbo.MedicalInfoProfiles",
                c => new
                    {
                        MedicalInfo_MedicalInfoID = c.Int(nullable: false),
                        Profile_ProfileID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MedicalInfo_MedicalInfoID, t.Profile_ProfileID })
                .ForeignKey("dbo.MedicalInfoes", t => t.MedicalInfo_MedicalInfoID, cascadeDelete: true)
                .ForeignKey("dbo.Profiles", t => t.Profile_ProfileID, cascadeDelete: true)
                .Index(t => t.MedicalInfo_MedicalInfoID)
                .Index(t => t.Profile_ProfileID);
            
            CreateTable(
                "dbo.PhotoProfiles",
                c => new
                    {
                        Photo_PhotoID = c.Int(nullable: false),
                        Profile_ProfileID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Photo_PhotoID, t.Profile_ProfileID })
                .ForeignKey("dbo.Photos", t => t.Photo_PhotoID, cascadeDelete: true)
                .ForeignKey("dbo.Profiles", t => t.Profile_ProfileID, cascadeDelete: true)
                .Index(t => t.Photo_PhotoID)
                .Index(t => t.Profile_ProfileID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.PhotoProfiles", "Profile_ProfileID", "dbo.Profiles");
            DropForeignKey("dbo.PhotoProfiles", "Photo_PhotoID", "dbo.Photos");
            DropForeignKey("dbo.MedicalInfoProfiles", "Profile_ProfileID", "dbo.Profiles");
            DropForeignKey("dbo.MedicalInfoProfiles", "MedicalInfo_MedicalInfoID", "dbo.MedicalInfoes");
            DropForeignKey("dbo.Profiles", "Admin_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.PhotoProfiles", new[] { "Profile_ProfileID" });
            DropIndex("dbo.PhotoProfiles", new[] { "Photo_PhotoID" });
            DropIndex("dbo.MedicalInfoProfiles", new[] { "Profile_ProfileID" });
            DropIndex("dbo.MedicalInfoProfiles", new[] { "MedicalInfo_MedicalInfoID" });
            DropIndex("dbo.Profiles", new[] { "Admin_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.PhotoProfiles");
            DropTable("dbo.MedicalInfoProfiles");
            DropTable("dbo.Photos");
            DropTable("dbo.MedicalInfoes");
            DropTable("dbo.Profiles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
        }
    }
}
