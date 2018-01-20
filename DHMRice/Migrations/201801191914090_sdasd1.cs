namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sdasd1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShopStocks", "ShopDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.ShopStocks", "Date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ShopStocks", "Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.ShopStocks", "ShopDate");
        }
    }
}
