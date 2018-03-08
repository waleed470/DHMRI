namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class additioninproductionworth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rice_Production_ProductWorth", "Rice_Production_ProductWorth_SoldQty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rice_Production_ProductWorth", "Rice_Production_ProductWorth_SoldQty");
        }
    }
}
