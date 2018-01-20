namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hfg : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ShopStocks", "ShopDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ShopStocks", "ShopDate", c => c.DateTime(nullable: false));
        }
    }
}
