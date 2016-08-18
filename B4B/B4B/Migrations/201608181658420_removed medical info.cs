namespace B4B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedmedicalinfo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MedicalInfoes", "ProfileID", "dbo.Profiles");
            DropIndex("dbo.MedicalInfoes", new[] { "ProfileID" });
            AddColumn("dbo.Profiles", "MedicalInformation", c => c.String());
            DropTable("dbo.MedicalInfoes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.MedicalInfoes",
                c => new
                    {
                        MedicalInfoID = c.Int(nullable: false, identity: true),
                        MedicalInformation = c.String(),
                        ProfileID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MedicalInfoID);
            
            DropColumn("dbo.Profiles", "MedicalInformation");
            CreateIndex("dbo.MedicalInfoes", "ProfileID");
            AddForeignKey("dbo.MedicalInfoes", "ProfileID", "dbo.Profiles", "ProfileID", cascadeDelete: true);
        }
    }
}
