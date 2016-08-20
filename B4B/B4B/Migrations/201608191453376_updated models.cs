namespace B4B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedmodels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MedicalInfoes",
                c => new
                    {
                        MedicalInfoID = c.Int(nullable: false, identity: true),
                        MedicalCondition = c.String(maxLength: 50),
                        Symptoms = c.String(maxLength: 150),
                        RelatedInformation = c.String(maxLength: 150),
                        ProfileID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MedicalInfoID)
                .ForeignKey("dbo.Profiles", t => t.ProfileID, cascadeDelete: true)
                .Index(t => t.ProfileID);
            
            DropColumn("dbo.Profiles", "MedicalInformation");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Profiles", "MedicalInformation", c => c.String());
            DropForeignKey("dbo.MedicalInfoes", "ProfileID", "dbo.Profiles");
            DropIndex("dbo.MedicalInfoes", new[] { "ProfileID" });
            DropTable("dbo.MedicalInfoes");
        }
    }
}
