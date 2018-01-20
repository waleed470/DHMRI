namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shopstock : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShopStocks",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ShopStockId = c.Int(nullable: false),
                        Shop_Id = c.Int(nullable: false),
                        Rice_Production_id = c.Int(nullable: false),
                        Qty = c.Int(nullable: false),
                        Packing_Id = c.Int(nullable: false),
                        SoldQty = c.Int(nullable: false),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NetTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Boolean(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Packings", t => t.Packing_Id, cascadeDelete: false)
                .ForeignKey("dbo.Rice_Production", t => t.Rice_Production_id, cascadeDelete: false)
                .ForeignKey("dbo.Shops", t => t.Shop_Id, cascadeDelete: true)
                .Index(t => t.Shop_Id)
                .Index(t => t.Rice_Production_id)
                .Index(t => t.Packing_Id)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShopStocks", "Shop_Id", "dbo.Shops");
            DropForeignKey("dbo.ShopStocks", "Rice_Production_id", "dbo.Rice_Production");
            DropForeignKey("dbo.ShopStocks", "Packing_Id", "dbo.Packings");
            DropForeignKey("dbo.ShopStocks", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ShopStocks", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ShopStocks", new[] { "Packing_Id" });
            DropIndex("dbo.ShopStocks", new[] { "Rice_Production_id" });
            DropIndex("dbo.ShopStocks", new[] { "Shop_Id" });
            DropTable("dbo.ShopStocks");
        }
    }
}
