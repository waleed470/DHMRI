namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class das : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShopStocks", "PerBagPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShopStocks", "PerBagPrice");
        }
    }
}
