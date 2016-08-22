namespace B4B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asterik : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profiles", "EcontactName", c => c.String(maxLength: 100));
            DropColumn("dbo.Profiles", "EcontactFirstName");
            DropColumn("dbo.Profiles", "EcontactLasttName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Profiles", "EcontactLasttName", c => c.String(maxLength: 50));
            AddColumn("dbo.Profiles", "EcontactFirstName", c => c.String(maxLength: 50));
            DropColumn("dbo.Profiles", "EcontactName");
        }
    }
}
