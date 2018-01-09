namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bagstableaddition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rice_Produce_Bag", "Rice_Produce_Bag_TotalMarketWorth", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Rice_Produce_Bag", "Rice_Produce_Bag_TotalMarketRawRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rice_Produce_Bag", "Rice_Produce_Bag_TotalMarketRawRate");
            DropColumn("dbo.Rice_Produce_Bag", "Rice_Produce_Bag_TotalMarketWorth");
        }
    }
}
