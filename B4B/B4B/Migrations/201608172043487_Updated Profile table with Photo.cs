namespace B4B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedProfiletablewithPhoto : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Photos", "PhotoID", "dbo.Profiles");
            DropIndex("dbo.Photos", new[] { "PhotoID" });
            AddColumn("dbo.Profiles", "PhotoName", c => c.String());
            AddColumn("dbo.Profiles", "PhotoType", c => c.String());
            AddColumn("dbo.Profiles", "PhotoBytes", c => c.Binary());
            AddColumn("dbo.Profiles", "FileType", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String());
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String());
            DropColumn("dbo.AspNetUsers", "AdminName");
            DropTable("dbo.Photos");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        PhotoID = c.Int(nullable: false),
                        PhotoName = c.String(maxLength: 50),
                        PhotoBytes = c.Binary(),
                        Extension = c.String(),
                    })
                .PrimaryKey(t => t.PhotoID);
            
            AddColumn("dbo.AspNetUsers", "AdminName", c => c.String());
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "FirstName");
            DropColumn("dbo.Profiles", "FileType");
            DropColumn("dbo.Profiles", "PhotoBytes");
            DropColumn("dbo.Profiles", "PhotoType");
            DropColumn("dbo.Profiles", "PhotoName");
            CreateIndex("dbo.Photos", "PhotoID");
            AddForeignKey("dbo.Photos", "PhotoID", "dbo.Profiles", "ProfileID");
        }
    }
}
