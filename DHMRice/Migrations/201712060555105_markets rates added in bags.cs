namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class marketsratesaddedinbags : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rice_Produce_Bag", "Rice_Produce_Bag_MarketFactoryCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Rice_Produce_Bag", "Rice_Produce_Bag_MarketStockWorth", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Rice_Produce_Bag", "Rice_Produce_Bag_MarketAverage", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rice_Produce_Bag", "Rice_Produce_Bag_MarketAverage");
            DropColumn("dbo.Rice_Produce_Bag", "Rice_Produce_Bag_MarketStockWorth");
            DropColumn("dbo.Rice_Produce_Bag", "Rice_Produce_Bag_MarketFactoryCost");
        }
    }
}
