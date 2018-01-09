namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesinproductionrices : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Production_Rice", "Market_Rate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Production_Rice", "Total_Rate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Production_Rice", "Total_Rate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Production_Rice", "Market_Rate");
        }
    }
}
