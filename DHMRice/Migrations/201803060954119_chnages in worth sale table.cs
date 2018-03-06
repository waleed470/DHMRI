namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chnagesinworthsaletable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BpRiceSales_ch", "Rice_Production_id", "dbo.Rice_Production");
            DropIndex("dbo.BpRiceSales_ch", new[] { "Rice_Production_id" });
            AddColumn("dbo.BpRiceSales_ch", "Rice_Production_ProductWorth_id", c => c.Int(nullable: false));
            AddColumn("dbo.Rice_Production_ProductWorth", "Rice_Production_ProductWorth_PBA", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            CreateIndex("dbo.BpRiceSales_ch", "Rice_Production_ProductWorth_id");
            AddForeignKey("dbo.BpRiceSales_ch", "Rice_Production_ProductWorth_id", "dbo.Rice_Production_ProductWorth", "Rice_Production_ProductWorth_id", cascadeDelete: true);
            DropColumn("dbo.BpRiceSales_ch", "Rice_Production_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BpRiceSales_ch", "Rice_Production_id", c => c.Int(nullable: false));
            DropForeignKey("dbo.BpRiceSales_ch", "Rice_Production_ProductWorth_id", "dbo.Rice_Production_ProductWorth");
            DropIndex("dbo.BpRiceSales_ch", new[] { "Rice_Production_ProductWorth_id" });
            DropColumn("dbo.Rice_Production_ProductWorth", "Rice_Production_ProductWorth_PBA");
            DropColumn("dbo.BpRiceSales_ch", "Rice_Production_ProductWorth_id");
            CreateIndex("dbo.BpRiceSales_ch", "Rice_Production_id");
            AddForeignKey("dbo.BpRiceSales_ch", "Rice_Production_id", "dbo.Rice_Production", "Rice_Production_id", cascadeDelete: true);
        }
    }
}
