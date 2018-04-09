namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shopstockchadditon : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShopStocks", "srsc_title", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShopStocks", "srsc_title");
        }
    }
}
