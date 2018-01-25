namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig_ab1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShopRiceSales_ch",
                c => new
                    {
                        srsc_id = c.Int(nullable: false, identity: true),
                        srsc_title = c.String(),
                        ShopStockId = c.Int(nullable: false),
                        srsc_qty = c.Int(nullable: false),
                        srsc_Weight_kg = c.Decimal(nullable: false, precision: 18, scale: 2),
                        srsc_Weight_mann = c.Decimal(nullable: false, precision: 18, scale: 2),
                        srsc_price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        srsp_id = c.Int(nullable: false),
                        srsc_status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.srsc_id)
                .ForeignKey("dbo.ShopRiceSales_pt", t => t.srsp_id, cascadeDelete: true)
                .ForeignKey("dbo.ShopStocks", t => t.ShopStockId, cascadeDelete: false)
                .Index(t => t.ShopStockId)
                .Index(t => t.srsp_id);
            
            CreateTable(
                "dbo.ShopRiceSales_pt",
                c => new
                    {
                        srsp_id = c.Int(nullable: false, identity: true),
                        srsp_Title = c.String(),
                        srsp_date = c.DateTime(nullable: false),
                        Party_Id = c.Int(nullable: false),
                        Id = c.String(maxLength: 128),
                        Shop_Id = c.Int(nullable: false),
                        srsp_TotalWeight_KG = c.Decimal(nullable: false, precision: 18, scale: 2),
                        srsp_TotalWeight_Mann = c.Decimal(nullable: false, precision: 18, scale: 2),
                        srsp_Total_Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        srsp_status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.srsp_id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.Parties", t => t.Party_Id, cascadeDelete: false)
                .ForeignKey("dbo.Shops", t => t.Shop_Id, cascadeDelete: true)
                .Index(t => t.Party_Id)
                .Index(t => t.Id)
                .Index(t => t.Shop_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShopRiceSales_ch", "ShopStockId", "dbo.ShopStocks");
            DropForeignKey("dbo.ShopRiceSales_ch", "srsp_id", "dbo.ShopRiceSales_pt");
            DropForeignKey("dbo.ShopRiceSales_pt", "Shop_Id", "dbo.Shops");
            DropForeignKey("dbo.ShopRiceSales_pt", "Party_Id", "dbo.Parties");
            DropForeignKey("dbo.ShopRiceSales_pt", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.ShopRiceSales_pt", new[] { "Shop_Id" });
            DropIndex("dbo.ShopRiceSales_pt", new[] { "Id" });
            DropIndex("dbo.ShopRiceSales_pt", new[] { "Party_Id" });
            DropIndex("dbo.ShopRiceSales_ch", new[] { "srsp_id" });
            DropIndex("dbo.ShopRiceSales_ch", new[] { "ShopStockId" });
            DropTable("dbo.ShopRiceSales_pt");
            DropTable("dbo.ShopRiceSales_ch");
        }
    }
}
