namespace B4B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedmodelsandaddedwizardmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MedicalInfoes", "MedicalCondition", c => c.String(maxLength: 50));
            AddColumn("dbo.MedicalInfoes", "Symptoms", c => c.String(maxLength: 150));
            AddColumn("dbo.MedicalInfoes", "RelatedInformation", c => c.String(maxLength: 150));
            DropColumn("dbo.MedicalInfoes", "MedicalInformation");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MedicalInfoes", "MedicalInformation", c => c.String());
            DropColumn("dbo.MedicalInfoes", "RelatedInformation");
            DropColumn("dbo.MedicalInfoes", "Symptoms");
            DropColumn("dbo.MedicalInfoes", "MedicalCondition");
        }
    }
}
