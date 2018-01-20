namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sdasd : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ShopStocks", "PerBagPrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ShopStocks", "PerBagPrice", c => c.Int(nullable: false));
        }
    }
}
