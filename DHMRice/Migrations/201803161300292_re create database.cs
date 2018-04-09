namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class recreatedatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BpRiceSales_ch",
                c => new
                    {
                        bprsc_id = c.Int(nullable: false, identity: true),
                        bprsc_title = c.String(),
                        Rice_Production_ProductWorth_id = c.Int(nullable: false),
                        bprsc_qty = c.Int(nullable: false),
                        bprsc_Weight_kg = c.Decimal(nullable: false, precision: 18, scale: 2),
                        bprsc_Weight_mann = c.Decimal(nullable: false, precision: 18, scale: 2),
                        bprsc_price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        bprsp_id = c.Int(nullable: false),
                        bprsc_status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.bprsc_id)
                .ForeignKey("dbo.BpRiceSales_pt", t => t.bprsp_id, cascadeDelete: true)
                .ForeignKey("dbo.Rice_Production_ProductWorth", t => t.Rice_Production_ProductWorth_id, cascadeDelete: false)
                .Index(t => t.Rice_Production_ProductWorth_id)
                .Index(t => t.bprsp_id);
            
            CreateTable(
                "dbo.BpRiceSales_pt",
                c => new
                    {
                        bprsp_id = c.Int(nullable: false, identity: true),
                        bprsp_Title = c.String(),
                        bprsp_date = c.DateTime(nullable: false),
                        Party_Id = c.Int(nullable: false),
                        Id = c.String(maxLength: 128),
                        bprsp_TotalWeight_KG = c.Decimal(nullable: false, precision: 18, scale: 2),
                        bprsp_TotalWeight_Mann = c.Decimal(nullable: false, precision: 18, scale: 2),
                        bprsp_Total_Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Carriage = c.Int(nullable: false),
                        Labour = c.Int(nullable: false),
                        bprsp_status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.bprsp_id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.Parties", t => t.Party_Id, cascadeDelete: false)
                .Index(t => t.Party_Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        MobileNo = c.String(),
                        User_Cnic = c.String(),
                        User_Adress = c.String(),
                        ShopCrendital = c.Boolean(nullable: false),
                        FactroryCrendital = c.Boolean(nullable: false),
                        Status = c.Boolean(nullable: false),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Parties",
                c => new
                    {
                        Party_Id = c.Int(nullable: false, identity: true),
                        Id = c.String(maxLength: 128),
                        Party_Name = c.String(nullable: false),
                        Party_Code = c.String(),
                        Party_MobileNo = c.String(nullable: false),
                        Party_BankName = c.String(),
                        Party_ACcountNo = c.String(),
                        Party_Address = c.String(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Party_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Rice_Production_ProductWorth",
                c => new
                    {
                        Rice_Production_ProductWorth_id = c.Int(nullable: false, identity: true),
                        Rice_Produce_Bags_id = c.Int(nullable: false),
                        Rice_Production_ProductWorth_name = c.String(),
                        Rice_Production_ProductWorth_Qty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Rice_Production_ProductWorth_SoldQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Rice_Production_ProductWorth_Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Rice_Production_ProductWorth_PBA = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Rice_Production_ProductWorth_id)
                .ForeignKey("dbo.Rice_Produce_Bag", t => t.Rice_Produce_Bags_id, cascadeDelete: true)
                .Index(t => t.Rice_Produce_Bags_id);
            
            CreateTable(
                "dbo.Rice_Produce_Bag",
                c => new
                    {
                        Rice_Produce_Bags_id = c.Int(nullable: false, identity: true),
                        Rice_Production_id = c.Int(nullable: false),
                        Rice_Produce_TotalBags = c.Int(nullable: false),
                        Rice_Produce_TotalBagsProduce = c.Int(nullable: false),
                        Rice_Produce_BagsSold = c.Int(nullable: false),
                        Rice_Produce_RemainingBags = c.Int(nullable: false),
                        Rice_Produce_Bag_ShortFall_total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Rice_Produce_Bag_TotalExpenses = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Rice_Produce_Bag_TotalBPW = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Rice_Produce_Bag_TotalWorth = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Rice_Produce_Bag_TotalMarketWorth = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Rice_Produce_Bag_FactoryCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Rice_Produce_Bag_MarketFactoryCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Rice_Produce_Bag_StockWorth = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Rice_Produce_Bag_MarketStockWorth = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Rice_Produce_Bag_Average = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Rice_Produce_Bag_MarketAverage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Rice_Produce_Bag_TotalWeight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Rice_Produce_Bag_PerBagPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Rice_Produce_Bag_TotalRawRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Rice_Produce_Bag_TotalMarketRawRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Rice_Produce_Bag_PerBagMarketPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Rice_Produce_Bag_Date = c.DateTime(nullable: false),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Rice_Produce_Bags_id)
                .ForeignKey("dbo.Rice_Production", t => t.Rice_Production_id, cascadeDelete: true)
                .Index(t => t.Rice_Production_id);
            
            CreateTable(
                "dbo.Rice_Production",
                c => new
                    {
                        Rice_Production_id = c.Int(nullable: false, identity: true),
                        Rice_Production_name = c.String(),
                        Rice_Production_Code = c.String(),
                        Id = c.String(maxLength: 128),
                        Packing_Id = c.Int(nullable: false),
                        Rice_Production_RemainingBags = c.Int(nullable: false),
                        Rice_Production_Date = c.DateTime(nullable: false),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Rice_Production_id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.Packings", t => t.Packing_Id, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.Packing_Id);
            
            CreateTable(
                "dbo.Packings",
                c => new
                    {
                        Packing_Id = c.Int(nullable: false, identity: true),
                        Id = c.String(maxLength: 128),
                        Packing_Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Packing_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Brokers",
                c => new
                    {
                        Broker_Id = c.Int(nullable: false, identity: true),
                        Id = c.String(maxLength: 128),
                        Broker_Name = c.String(nullable: false),
                        Broker_Code = c.String(),
                        Broker_MobileNo = c.String(),
                        Broker_BankName = c.String(),
                        Broker_ACcountNo = c.String(),
                        Broker_Address = c.String(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Broker_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Customer_Id = c.Int(nullable: false, identity: true),
                        Customer_Name = c.String(nullable: false),
                        Id = c.String(maxLength: 128),
                        Customer_Code = c.String(nullable: false),
                        Customer_MobileNo = c.String(nullable: false),
                        Customer_BankName = c.String(),
                        Customer_ACcountNo = c.String(),
                        Customer_Address = c.String(nullable: false),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Customer_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Factories",
                c => new
                    {
                        Factory_Id = c.Int(nullable: false, identity: true),
                        Factory_Name = c.String(nullable: false),
                        Id = c.String(maxLength: 128),
                        Factory_Code = c.String(),
                        Factory_Number = c.String(nullable: false),
                        Factory_BankName = c.String(nullable: false),
                        Factory_ACcountNo = c.String(nullable: false),
                        Factory_Address = c.String(nullable: false),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Factory_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
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
                "dbo.RawRices",
                c => new
                    {
                        RawRice_id = c.Int(nullable: false, identity: true),
                        Party_Id = c.Int(nullable: false),
                        Id = c.String(nullable: false, maxLength: 128),
                        Broker_Id = c.Int(nullable: false),
                        Rice_category_Id = c.Int(nullable: false),
                        Item_Name = c.String(nullable: false),
                        Item_Code = c.String(nullable: false),
                        Packing_Id = c.Int(nullable: false),
                        Bags_qty = c.Int(nullable: false),
                        Bags_Sold_qty = c.Int(nullable: false),
                        Total_Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Pb_Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total_Mann = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Pay_CommissionPercentage = c.Boolean(nullable: false),
                        BrokerCommissionPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BrokerCommissionAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RawRice_id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id, cascadeDelete: false)
                .ForeignKey("dbo.Brokers", t => t.Broker_Id, cascadeDelete: false)
                .ForeignKey("dbo.Packings", t => t.Packing_Id, cascadeDelete: false)
                .ForeignKey("dbo.Parties", t => t.Party_Id, cascadeDelete: false)
                .ForeignKey("dbo.Rice_Category", t => t.Rice_category_Id, cascadeDelete: true)
                .Index(t => t.Party_Id)
                .Index(t => t.Id)
                .Index(t => t.Broker_Id)
                .Index(t => t.Rice_category_Id)
                .Index(t => t.Packing_Id);
            
            CreateTable(
                "dbo.Rice_Category",
                c => new
                    {
                        Rice_category_Id = c.Int(nullable: false, identity: true),
                        Rice_Category_Name = c.String(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Rice_category_Id);
            
            CreateTable(
                "dbo.GatePassOutwards",
                c => new
                    {
                        GatePassOutwardId = c.Int(nullable: false, identity: true),
                        RiceTypeId = c.Int(nullable: false),
                        RiceType = c.String(),
                        Vehicle_No = c.String(),
                        Driver_Name = c.String(),
                        Bility_No = c.Int(nullable: false),
                        Designation = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.GatePassOutwardId);
            
            CreateTable(
                "dbo.Opening_ClosingDays",
                c => new
                    {
                        Opening_ClosingDays_id = c.Int(nullable: false, identity: true),
                        Opening_Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Closing_Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false),
                        isClosed = c.Boolean(nullable: false),
                        status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Opening_ClosingDays_id);
            
            CreateTable(
                "dbo.Opening_ClosingDays_Shop",
                c => new
                    {
                        Opening_ClosingDays_Shop_id = c.Int(nullable: false, identity: true),
                        Opening_Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Closing_Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false),
                        isClosed = c.Boolean(nullable: false),
                        Shop_Id = c.Int(nullable: false),
                        status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Opening_ClosingDays_Shop_id)
                .ForeignKey("dbo.Shops", t => t.Shop_Id, cascadeDelete: true)
                .Index(t => t.Shop_Id);
            
            CreateTable(
                "dbo.Shops",
                c => new
                    {
                        Shop_Id = c.Int(nullable: false, identity: true),
                        Shop_Name = c.String(nullable: false),
                        Id = c.String(maxLength: 128),
                        Shop_Code = c.String(),
                        Shop_MobileNo = c.String(),
                        Shop_Address = c.String(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Shop_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.PartyRemainings",
                c => new
                    {
                        PartyRemaining_Id = c.Int(nullable: false, identity: true),
                        Party_Id = c.Int(nullable: false),
                        RemainingType = c.String(),
                        isPayed = c.Boolean(nullable: false),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RemainingAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.PartyRemaining_Id)
                .ForeignKey("dbo.Parties", t => t.Party_Id, cascadeDelete: true)
                .Index(t => t.Party_Id);
            
            CreateTable(
                "dbo.Pricings",
                c => new
                    {
                        Pricing_id = c.Int(nullable: false, identity: true),
                        item_id = c.Int(nullable: false),
                        item_Type = c.String(),
                        Pricing_Rate_kg = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Pricing_Rate_Maan = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Pricing_Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PerBagPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PerBagMarketPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Pricing_NetTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Pricing_Date = c.DateTime(nullable: false),
                        Pricing_ModifiedDate = c.DateTime(nullable: false),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Pricing_id);
            
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
                        Id = c.String(maxLength: 128),
                        prsp_TotalWeight_KG = c.Decimal(nullable: false, precision: 18, scale: 2),
                        prsp_TotalWeight_Mann = c.Decimal(nullable: false, precision: 18, scale: 2),
                        prsp_Total_Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Carriage = c.Int(nullable: false),
                        Labour = c.Int(nullable: false),
                        prsp_status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.prsp_id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.Parties", t => t.Party_Id, cascadeDelete: true)
                .Index(t => t.Party_Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Production_Extra_Rice",
                c => new
                    {
                        Production_Extra_Rice_id = c.Int(nullable: false, identity: true),
                        Rice_Produce_Bags_id = c.Int(nullable: false),
                        Extra_Rice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Production_Extra_Rice_id)
                .ForeignKey("dbo.Rice_Produce_Bag", t => t.Rice_Produce_Bags_id, cascadeDelete: true)
                .Index(t => t.Rice_Produce_Bags_id);
            
            CreateTable(
                "dbo.Production_Rice",
                c => new
                    {
                        Production_Rice_id = c.Int(nullable: false, identity: true),
                        Rice_Produce_Bags_id = c.Int(nullable: false),
                        RawRice_id = c.Int(nullable: false),
                        Purchase_Rice_Rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Purchase_Rice_BagsUsed = c.Int(nullable: false),
                        Packing_Id = c.Int(nullable: false),
                        Total_Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total_Worth = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Market_Worth = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Market_Rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Production_Rice_id)
                .ForeignKey("dbo.RawRices", t => t.RawRice_id, cascadeDelete: false)
                .ForeignKey("dbo.Rice_Produce_Bag", t => t.Rice_Produce_Bags_id, cascadeDelete: true)
                .Index(t => t.Rice_Produce_Bags_id)
                .Index(t => t.RawRice_id);
            
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
                        Carriage = c.Int(nullable: false),
                        Labour = c.Int(nullable: false),
                        rsp_status = c.Boolean(nullable: false),
                        Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.rsp_id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.Parties", t => t.Party_Id, cascadeDelete: true)
                .Index(t => t.Party_Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.RawRiceExpenses",
                c => new
                    {
                        RawRiceExpense_id = c.Int(nullable: false, identity: true),
                        RawRiceExpense_Name = c.String(),
                        RawRiceExpense_Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RawRice_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RawRiceExpense_id)
                .ForeignKey("dbo.RawRices", t => t.RawRice_id, cascadeDelete: true)
                .Index(t => t.RawRice_id);
            
            CreateTable(
                "dbo.Rice_Production_Expense",
                c => new
                    {
                        Rice_Production_Expense_id = c.Int(nullable: false, identity: true),
                        Rice_Produce_Bags_id = c.Int(nullable: false),
                        Rice_Production_Expense_name = c.String(),
                        Rice_Production_Expense_Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Rice_Production_Expense_id)
                .ForeignKey("dbo.Rice_Produce_Bag", t => t.Rice_Produce_Bags_id, cascadeDelete: true)
                .Index(t => t.Rice_Produce_Bags_id);
            
            CreateTable(
                "dbo.Rice_Production_ShortFall",
                c => new
                    {
                        Rice_Production_ShortFall_id = c.Int(nullable: false, identity: true),
                        Rice_Produce_Bags_id = c.Int(nullable: false),
                        Rice_Production_ShortFall_name = c.String(),
                        Rice_Production_ShortFall_Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Rice_Production_ShortFall_id)
                .ForeignKey("dbo.Rice_Produce_Bag", t => t.Rice_Produce_Bags_id, cascadeDelete: true)
                .Index(t => t.Rice_Produce_Bags_id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.SaleInvoices",
                c => new
                    {
                        SaleInvoice_id = c.Int(nullable: false, identity: true),
                        SaleInvoice_no = c.Int(nullable: false),
                        Sale_id = c.Int(nullable: false),
                        SaleInvoice_type = c.String(),
                    })
                .PrimaryKey(t => t.SaleInvoice_id);
            
            CreateTable(
                "dbo.Shop_Account",
                c => new
                    {
                        Shop_AccountId = c.Int(nullable: false, identity: true),
                        Shop_Id = c.Int(nullable: false),
                        Shop_BankName = c.String(nullable: false),
                        Shop_ACcountNo = c.String(nullable: false),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Shop_AccountId)
                .ForeignKey("dbo.Shops", t => t.Shop_Id, cascadeDelete: true)
                .Index(t => t.Shop_Id);
            
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
                .ForeignKey("dbo.ShopRiceSales_pt", t => t.srsp_id, cascadeDelete: false)
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
                        Customer_Id = c.Int(nullable: false),
                        Id = c.String(maxLength: 128),
                        Shop_Id = c.Int(nullable: false),
                        srsp_TotalWeight_KG = c.Decimal(nullable: false, precision: 18, scale: 2),
                        srsp_TotalWeight_Mann = c.Decimal(nullable: false, precision: 18, scale: 2),
                        srsp_Total_Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        srsp_status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.srsp_id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id, cascadeDelete: false)
                .ForeignKey("dbo.Shops", t => t.Shop_Id, cascadeDelete: false)
                .Index(t => t.Customer_Id)
                .Index(t => t.Id)
                .Index(t => t.Shop_Id);
            
            CreateTable(
                "dbo.ShopStocks",
                c => new
                    {
                        ShopStockId = c.Int(nullable: false, identity: true),
                        Rice_Production_id = c.Int(nullable: false),
                        srsp_id = c.Int(nullable: false),
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
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ShopStockId)
                .ForeignKey("dbo.Rice_Production", t => t.Rice_Production_id, cascadeDelete: false)
                .ForeignKey("dbo.ShopStock_pt", t => t.srsp_id, cascadeDelete: true)
                .Index(t => t.Rice_Production_id)
                .Index(t => t.srsp_id);
            
            CreateTable(
                "dbo.ShopStock_pt",
                c => new
                    {
                        srsp_id = c.Int(nullable: false, identity: true),
                        srsp_Title = c.String(),
                        srsp_date = c.DateTime(nullable: false),
                        Shop_Id = c.Int(nullable: false),
                        Id = c.String(maxLength: 128),
                        srsp_TotalWeight_KG = c.Decimal(nullable: false, precision: 18, scale: 2),
                        srsp_TotalWeight_Mann = c.Decimal(nullable: false, precision: 18, scale: 2),
                        srsp_Total_Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Carriage = c.Int(nullable: false),
                        Labour = c.Int(nullable: false),
                        srsp_status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.srsp_id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.Shops", t => t.Shop_Id, cascadeDelete: true)
                .Index(t => t.Shop_Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Transaction_id = c.Int(nullable: false, identity: true),
                        Transaction_item_id = c.Int(nullable: false),
                        Transaction_item_type = c.String(),
                        Transaction_Description = c.String(),
                        isByCash = c.Boolean(nullable: false),
                        BankAccountNo = c.String(),
                        checkno = c.Int(nullable: false),
                        Debit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Credit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Transaction_DateTime = c.DateTime(nullable: false),
                        Opening_ClosingDays_id = c.Int(nullable: false),
                        status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Transaction_id)
                .ForeignKey("dbo.Opening_ClosingDays", t => t.Opening_ClosingDays_id, cascadeDelete: true)
                .Index(t => t.Opening_ClosingDays_id);
            
            CreateTable(
                "dbo.Transaction_Shop",
                c => new
                    {
                        Transaction_Shop_id = c.Int(nullable: false, identity: true),
                        Transaction_Shop_item_id = c.Int(nullable: false),
                        Transaction_Shop_item_type = c.String(),
                        Transaction_Shop_Description = c.String(),
                        isByCash = c.Boolean(nullable: false),
                        BankAccountNo = c.String(),
                        checkno = c.Int(nullable: false),
                        Debit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Credit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Transaction_Shop_DateTime = c.DateTime(nullable: false),
                        Opening_ClosingDays_Shop_id = c.Int(nullable: false),
                        status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Transaction_Shop_id)
                .ForeignKey("dbo.Opening_ClosingDays_Shop", t => t.Opening_ClosingDays_Shop_id, cascadeDelete: true)
                .Index(t => t.Opening_ClosingDays_Shop_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transaction_Shop", "Opening_ClosingDays_Shop_id", "dbo.Opening_ClosingDays_Shop");
            DropForeignKey("dbo.Transactions", "Opening_ClosingDays_id", "dbo.Opening_ClosingDays");
            DropForeignKey("dbo.ShopRiceSales_ch", "ShopStockId", "dbo.ShopStocks");
            DropForeignKey("dbo.ShopStocks", "srsp_id", "dbo.ShopStock_pt");
            DropForeignKey("dbo.ShopStock_pt", "Shop_Id", "dbo.Shops");
            DropForeignKey("dbo.ShopStock_pt", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ShopStocks", "Rice_Production_id", "dbo.Rice_Production");
            DropForeignKey("dbo.ShopRiceSales_ch", "srsp_id", "dbo.ShopRiceSales_pt");
            DropForeignKey("dbo.ShopRiceSales_pt", "Shop_Id", "dbo.Shops");
            DropForeignKey("dbo.ShopRiceSales_pt", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.ShopRiceSales_pt", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Shop_Account", "Shop_Id", "dbo.Shops");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Rice_Production_ShortFall", "Rice_Produce_Bags_id", "dbo.Rice_Produce_Bag");
            DropForeignKey("dbo.Rice_Production_Expense", "Rice_Produce_Bags_id", "dbo.Rice_Produce_Bag");
            DropForeignKey("dbo.RawRiceExpenses", "RawRice_id", "dbo.RawRices");
            DropForeignKey("dbo.RawRice_Sales_ch", "rsp_id", "dbo.RawRice_Sales_pt");
            DropForeignKey("dbo.RawRice_Sales_pt", "Party_Id", "dbo.Parties");
            DropForeignKey("dbo.RawRice_Sales_pt", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.RawRice_Sales_ch", "RawRice_id", "dbo.RawRices");
            DropForeignKey("dbo.Production_Rice", "Rice_Produce_Bags_id", "dbo.Rice_Produce_Bag");
            DropForeignKey("dbo.Production_Rice", "RawRice_id", "dbo.RawRices");
            DropForeignKey("dbo.Production_Extra_Rice", "Rice_Produce_Bags_id", "dbo.Rice_Produce_Bag");
            DropForeignKey("dbo.ProducedRiceSales_ch", "Rice_Production_id", "dbo.Rice_Production");
            DropForeignKey("dbo.ProducedRiceSales_ch", "prsp_id", "dbo.ProducedRiceSales_pt");
            DropForeignKey("dbo.ProducedRiceSales_pt", "Party_Id", "dbo.Parties");
            DropForeignKey("dbo.ProducedRiceSales_pt", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PartyRemainings", "Party_Id", "dbo.Parties");
            DropForeignKey("dbo.Opening_ClosingDays_Shop", "Shop_Id", "dbo.Shops");
            DropForeignKey("dbo.Shops", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.GatePassInwareds", "RawRice_id", "dbo.RawRices");
            DropForeignKey("dbo.RawRices", "Rice_category_Id", "dbo.Rice_Category");
            DropForeignKey("dbo.RawRices", "Party_Id", "dbo.Parties");
            DropForeignKey("dbo.RawRices", "Packing_Id", "dbo.Packings");
            DropForeignKey("dbo.RawRices", "Broker_Id", "dbo.Brokers");
            DropForeignKey("dbo.RawRices", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Factories", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Customers", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Brokers", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.BpRiceSales_ch", "Rice_Production_ProductWorth_id", "dbo.Rice_Production_ProductWorth");
            DropForeignKey("dbo.Rice_Production_ProductWorth", "Rice_Produce_Bags_id", "dbo.Rice_Produce_Bag");
            DropForeignKey("dbo.Rice_Produce_Bag", "Rice_Production_id", "dbo.Rice_Production");
            DropForeignKey("dbo.Rice_Production", "Packing_Id", "dbo.Packings");
            DropForeignKey("dbo.Packings", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Rice_Production", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.BpRiceSales_ch", "bprsp_id", "dbo.BpRiceSales_pt");
            DropForeignKey("dbo.BpRiceSales_pt", "Party_Id", "dbo.Parties");
            DropForeignKey("dbo.Parties", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.BpRiceSales_pt", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Transaction_Shop", new[] { "Opening_ClosingDays_Shop_id" });
            DropIndex("dbo.Transactions", new[] { "Opening_ClosingDays_id" });
            DropIndex("dbo.ShopStock_pt", new[] { "Id" });
            DropIndex("dbo.ShopStock_pt", new[] { "Shop_Id" });
            DropIndex("dbo.ShopStocks", new[] { "srsp_id" });
            DropIndex("dbo.ShopStocks", new[] { "Rice_Production_id" });
            DropIndex("dbo.ShopRiceSales_pt", new[] { "Shop_Id" });
            DropIndex("dbo.ShopRiceSales_pt", new[] { "Id" });
            DropIndex("dbo.ShopRiceSales_pt", new[] { "Customer_Id" });
            DropIndex("dbo.ShopRiceSales_ch", new[] { "srsp_id" });
            DropIndex("dbo.ShopRiceSales_ch", new[] { "ShopStockId" });
            DropIndex("dbo.Shop_Account", new[] { "Shop_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Rice_Production_ShortFall", new[] { "Rice_Produce_Bags_id" });
            DropIndex("dbo.Rice_Production_Expense", new[] { "Rice_Produce_Bags_id" });
            DropIndex("dbo.RawRiceExpenses", new[] { "RawRice_id" });
            DropIndex("dbo.RawRice_Sales_pt", new[] { "Id" });
            DropIndex("dbo.RawRice_Sales_pt", new[] { "Party_Id" });
            DropIndex("dbo.RawRice_Sales_ch", new[] { "rsp_id" });
            DropIndex("dbo.RawRice_Sales_ch", new[] { "RawRice_id" });
            DropIndex("dbo.Production_Rice", new[] { "RawRice_id" });
            DropIndex("dbo.Production_Rice", new[] { "Rice_Produce_Bags_id" });
            DropIndex("dbo.Production_Extra_Rice", new[] { "Rice_Produce_Bags_id" });
            DropIndex("dbo.ProducedRiceSales_pt", new[] { "Id" });
            DropIndex("dbo.ProducedRiceSales_pt", new[] { "Party_Id" });
            DropIndex("dbo.ProducedRiceSales_ch", new[] { "prsp_id" });
            DropIndex("dbo.ProducedRiceSales_ch", new[] { "Rice_Production_id" });
            DropIndex("dbo.PartyRemainings", new[] { "Party_Id" });
            DropIndex("dbo.Shops", new[] { "Id" });
            DropIndex("dbo.Opening_ClosingDays_Shop", new[] { "Shop_Id" });
            DropIndex("dbo.RawRices", new[] { "Packing_Id" });
            DropIndex("dbo.RawRices", new[] { "Rice_category_Id" });
            DropIndex("dbo.RawRices", new[] { "Broker_Id" });
            DropIndex("dbo.RawRices", new[] { "Id" });
            DropIndex("dbo.RawRices", new[] { "Party_Id" });
            DropIndex("dbo.GatePassInwareds", new[] { "RawRice_id" });
            DropIndex("dbo.Factories", new[] { "Id" });
            DropIndex("dbo.Customers", new[] { "Id" });
            DropIndex("dbo.Brokers", new[] { "Id" });
            DropIndex("dbo.Packings", new[] { "Id" });
            DropIndex("dbo.Rice_Production", new[] { "Packing_Id" });
            DropIndex("dbo.Rice_Production", new[] { "Id" });
            DropIndex("dbo.Rice_Produce_Bag", new[] { "Rice_Production_id" });
            DropIndex("dbo.Rice_Production_ProductWorth", new[] { "Rice_Produce_Bags_id" });
            DropIndex("dbo.Parties", new[] { "Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.BpRiceSales_pt", new[] { "Id" });
            DropIndex("dbo.BpRiceSales_pt", new[] { "Party_Id" });
            DropIndex("dbo.BpRiceSales_ch", new[] { "bprsp_id" });
            DropIndex("dbo.BpRiceSales_ch", new[] { "Rice_Production_ProductWorth_id" });
            DropTable("dbo.Transaction_Shop");
            DropTable("dbo.Transactions");
            DropTable("dbo.ShopStock_pt");
            DropTable("dbo.ShopStocks");
            DropTable("dbo.ShopRiceSales_pt");
            DropTable("dbo.ShopRiceSales_ch");
            DropTable("dbo.Shop_Account");
            DropTable("dbo.SaleInvoices");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Rice_Production_ShortFall");
            DropTable("dbo.Rice_Production_Expense");
            DropTable("dbo.RawRiceExpenses");
            DropTable("dbo.RawRice_Sales_pt");
            DropTable("dbo.RawRice_Sales_ch");
            DropTable("dbo.Production_Rice");
            DropTable("dbo.Production_Extra_Rice");
            DropTable("dbo.ProducedRiceSales_pt");
            DropTable("dbo.ProducedRiceSales_ch");
            DropTable("dbo.Pricings");
            DropTable("dbo.PartyRemainings");
            DropTable("dbo.Shops");
            DropTable("dbo.Opening_ClosingDays_Shop");
            DropTable("dbo.Opening_ClosingDays");
            DropTable("dbo.GatePassOutwards");
            DropTable("dbo.Rice_Category");
            DropTable("dbo.RawRices");
            DropTable("dbo.GatePassInwareds");
            DropTable("dbo.Factories");
            DropTable("dbo.Customers");
            DropTable("dbo.Brokers");
            DropTable("dbo.Packings");
            DropTable("dbo.Rice_Production");
            DropTable("dbo.Rice_Produce_Bag");
            DropTable("dbo.Rice_Production_ProductWorth");
            DropTable("dbo.Parties");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.BpRiceSales_pt");
            DropTable("dbo.BpRiceSales_ch");
        }
    }
}
