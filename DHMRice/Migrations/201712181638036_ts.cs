namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GatePassInwareds",
                c => new
                    {
                        GatePassInwaredId = c.Int(nullable: false, identity: true),
                        RawRice_id = c.Int(nullable: false),
                        Vehicle_No = c.String(),
                        Driver_Name = c.String(),
                        Bility_No = c.Int(nullable: false),
                        Purchased_By = c.String(),
                        Designation = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.GatePassInwaredId)
                .ForeignKey("dbo.RawRices", t => t.RawRice_id, cascadeDelete: true)
                .Index(t => t.RawRice_id);
            
            CreateTable(
                "dbo.ProducedRiceSales_ch",
                c => new
                    {
                        prsc_id = c.Int(nullable: false, identity: true),
                        prsc_title = c.String(),
                        Rice_Production_id = c.Int(nullable: false),
                        prsc_qty = c.Int(nullable: false),
                        prsc_Weight_kg = c.Decimal(nullable: false, precision: 18, scale: 2),
                        prsc_Weight_mann = c.Decimal(nullable: false, precision: 18, scale: 2),
                        prsc_price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        prsp_id = c.Int(nullable: false),
                        prsc_status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.prsc_id)
                .ForeignKey("dbo.ProducedRiceSales_pt", t => t.prsp_id, cascadeDelete: true)
                .ForeignKey("dbo.Rice_Production", t => t.Rice_Production_id, cascadeDelete: true)
                .Index(t => t.Rice_Production_id)
                .Index(t => t.prsp_id);
            
            CreateTable(
                "dbo.ProducedRiceSales_pt",
                c => new
                    {
                        prsp_id = c.Int(nullable: false, identity: true),
                        prsp_Title = c.String(),
                        prsp_date = c.DateTime(nullable: false),
                        Party_Id = c.Int(nullable: false),
                        prsp_TotalWeight_KG = c.Decimal(nullable: false, precision: 18, scale: 2),
                        prsp_TotalWeight_Mann = c.Decimal(nullable: false, precision: 18, scale: 2),
                        prsp_Total_Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        prsp_status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.prsp_id)
                .ForeignKey("dbo.Parties", t => t.Party_Id, cascadeDelete: true)
                .Index(t => t.Party_Id);
            
            CreateTable(
                "dbo.RawRice_Sales_ch",
                c => new
                    {
                        rsc_id = c.Int(nullable: false, identity: true),
                        rsc_title = c.String(),
                        RawRice_id = c.Int(nullable: false),
                        rsc_qty = c.Int(nullable: false),
                        rsc_Weight_kg = c.Decimal(nullable: false, precision: 18, scale: 2),
                        rsc_Weight_mann = c.Decimal(nullable: false, precision: 18, scale: 2),
                        rsc_price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        rsp_id = c.Int(nullable: false),
                        rsc_status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.rsc_id)
                .ForeignKey("dbo.RawRices", t => t.RawRice_id, cascadeDelete: true)
                .ForeignKey("dbo.RawRice_Sales_pt", t => t.rsp_id, cascadeDelete: true)
                .Index(t => t.RawRice_id)
                .Index(t => t.rsp_id);
            
            CreateTable(
                "dbo.RawRice_Sales_pt",
                c => new
                    {
                        rsp_id = c.Int(nullable: false, identity: true),
                        rsp_Title = c.String(),
                        rsp_date = c.DateTime(nullable: false),
                        Party_Id = c.Int(nullable: false),
                        rsp_TotalWeight_KG = c.Decimal(nullable: false, precision: 18, scale: 2),
                        rsp_TotalWeight_Mann = c.Decimal(nullable: false, precision: 18, scale: 2),
                        rsp_Total_Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        rsp_status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.rsp_id)
                .ForeignKey("dbo.Parties", t => t.Party_Id, cascadeDelete: true)
                .Index(t => t.Party_Id);
            
            AddColumn("dbo.Rice_Produce_Bag", "Rice_Produce_Bag_TotalMarketWorth", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Rice_Produce_Bag", "Rice_Produce_Bag_MarketFactoryCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Rice_Produce_Bag", "Rice_Produce_Bag_MarketStockWorth", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Rice_Produce_Bag", "Rice_Produce_Bag_MarketAverage", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Rice_Produce_Bag", "Rice_Produce_Bag_TotalMarketRawRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Production_Rice", "Market_Worth", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Production_Rice", "Market_Rate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RawRice_Sales_ch", "rsp_id", "dbo.RawRice_Sales_pt");
            DropForeignKey("dbo.RawRice_Sales_pt", "Party_Id", "dbo.Parties");
            DropForeignKey("dbo.RawRice_Sales_ch", "RawRice_id", "dbo.RawRices");
            DropForeignKey("dbo.ProducedRiceSales_ch", "Rice_Production_id", "dbo.Rice_Production");
            DropForeignKey("dbo.ProducedRiceSales_ch", "prsp_id", "dbo.ProducedRiceSales_pt");
            DropForeignKey("dbo.ProducedRiceSales_pt", "Party_Id", "dbo.Parties");
            DropForeignKey("dbo.GatePassInwareds", "RawRice_id", "dbo.RawRices");
            DropIndex("dbo.RawRice_Sales_pt", new[] { "Party_Id" });
            DropIndex("dbo.RawRice_Sales_ch", new[] { "rsp_id" });
            DropIndex("dbo.RawRice_Sales_ch", new[] { "RawRice_id" });
            DropIndex("dbo.ProducedRiceSales_pt", new[] { "Party_Id" });
            DropIndex("dbo.ProducedRiceSales_ch", new[] { "prsp_id" });
            DropIndex("dbo.ProducedRiceSales_ch", new[] { "Rice_Production_id" });
            DropIndex("dbo.GatePassInwareds", new[] { "RawRice_id" });
            DropColumn("dbo.Production_Rice", "Market_Rate");
            DropColumn("dbo.Production_Rice", "Market_Worth");
            DropColumn("dbo.Rice_Produce_Bag", "Rice_Produce_Bag_TotalMarketRawRate");
            DropColumn("dbo.Rice_Produce_Bag", "Rice_Produce_Bag_MarketAverage");
            DropColumn("dbo.Rice_Produce_Bag", "Rice_Produce_Bag_MarketStockWorth");
            DropColumn("dbo.Rice_Produce_Bag", "Rice_Produce_Bag_MarketFactoryCost");
            DropColumn("dbo.Rice_Produce_Bag", "Rice_Produce_Bag_TotalMarketWorth");
            DropTable("dbo.RawRice_Sales_pt");
            DropTable("dbo.RawRice_Sales_ch");
            DropTable("dbo.ProducedRiceSales_pt");
            DropTable("dbo.ProducedRiceSales_ch");
            DropTable("dbo.GatePassInwareds");
        }
    }
}
