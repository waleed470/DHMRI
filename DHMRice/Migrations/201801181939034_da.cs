namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class da : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ShopStocks", "Rate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ShopStocks", "Rate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
