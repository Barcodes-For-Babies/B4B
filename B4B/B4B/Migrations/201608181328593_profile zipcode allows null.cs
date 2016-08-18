namespace B4B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class profilezipcodeallowsnull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Profiles", "ZipCode", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Profiles", "ZipCode", c => c.Int(nullable: false));
        }
    }
}
