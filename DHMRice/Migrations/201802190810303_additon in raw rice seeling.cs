namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class additoninrawriceseeling : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RawRice_Sales_pt", "Carriage", c => c.Int(nullable: false));
            AddColumn("dbo.RawRice_Sales_pt", "Labour", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RawRice_Sales_pt", "Labour");
            DropColumn("dbo.RawRice_Sales_pt", "Carriage");
        }
    }
}
