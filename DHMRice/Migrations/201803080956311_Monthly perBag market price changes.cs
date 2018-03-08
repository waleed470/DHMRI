namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MonthlyperBagmarketpricechanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pricings", "Pricing_ModifiedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pricings", "Pricing_ModifiedDate");
        }
    }
}
