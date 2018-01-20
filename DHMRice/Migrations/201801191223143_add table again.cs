namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtableagain : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShopStocks",
                c => new
                    {
                        ShopStockId = c.Int(nullable: false, identity: true),
                        Shop_Id = c.Int(nullable: false),
                        Id = c.String(maxLength: 128),
                        Rice_Production_id = c.Int(nullable: false),
                        Qty = c.Int(nullable: false),
                        packing_type = c.Int(nullable: false),
                        SoldQty = c.Int(nullable: false),
                        PerBagPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SumWeight_KG = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalWeight_KG = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SumWeight_Mann = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalWeight_Mann = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total_Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NetTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ShopStockId)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.Rice_Production", t => t.Rice_Production_id, cascadeDelete: false)
                .ForeignKey("dbo.Shops", t => t.Shop_Id, cascadeDelete: true)
                .Index(t => t.Shop_Id)
                .Index(t => t.Id)
                .Index(t => t.Rice_Production_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShopStocks", "Shop_Id", "dbo.Shops");
            DropForeignKey("dbo.ShopStocks", "Rice_Production_id", "dbo.Rice_Production");
            DropForeignKey("dbo.ShopStocks", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.ShopStocks", new[] { "Rice_Production_id" });
            DropIndex("dbo.ShopStocks", new[] { "Id" });
            DropIndex("dbo.ShopStocks", new[] { "Shop_Id" });
            DropTable("dbo.ShopStocks");
        }
    }
}
