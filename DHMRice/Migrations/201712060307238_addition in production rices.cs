namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class additioninproductionrices : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Production_Rice", "Market_Worth", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Production_Rice", "Total_Rate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Production_Rice", "Total_Rate");
            DropColumn("dbo.Production_Rice", "Market_Worth");
        }
    }
}
