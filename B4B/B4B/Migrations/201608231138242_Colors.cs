namespace B4B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Colors : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profiles", "forgroundColor", c => c.Int(nullable: false));
            AddColumn("dbo.Profiles", "backgroundColor", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Profiles", "backgroundColor");
            DropColumn("dbo.Profiles", "forgroundColor");
        }
    }
}
