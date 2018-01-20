namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class additioninshopstock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShopStocks", "TotalWeight_KG", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ShopStocks", "TotalWeight_Mann", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ShopStocks", "Total_Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ShopStocks", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShopStocks", "Date");
            DropColumn("dbo.ShopStocks", "Total_Amount");
            DropColumn("dbo.ShopStocks", "TotalWeight_Mann");
            DropColumn("dbo.ShopStocks", "TotalWeight_KG");
        }
    }
}
