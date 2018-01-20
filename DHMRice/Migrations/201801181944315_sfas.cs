namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sfas : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ShopStocks", "Packing_Id", "dbo.Packings");
            DropIndex("dbo.ShopStocks", new[] { "Packing_Id" });
            AddColumn("dbo.ShopStocks", "packing_type", c => c.Int(nullable: false));
            DropColumn("dbo.ShopStocks", "Packing_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ShopStocks", "Packing_Id", c => c.Int(nullable: false));
            DropColumn("dbo.ShopStocks", "packing_type");
            CreateIndex("dbo.ShopStocks", "Packing_Id");
            AddForeignKey("dbo.ShopStocks", "Packing_Id", "dbo.Packings", "Packing_Id", cascadeDelete: true);
        }
    }
}
