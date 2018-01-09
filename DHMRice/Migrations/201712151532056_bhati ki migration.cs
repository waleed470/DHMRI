namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bhatikimigration : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProducedRiceSales_ch", "Rice_Production_id", "dbo.Rice_Production");
            DropForeignKey("dbo.ProducedRiceSales_ch", "prsp_id", "dbo.ProducedRiceSales_pt");
            DropForeignKey("dbo.ProducedRiceSales_pt", "Party_Id", "dbo.Parties");
            DropIndex("dbo.ProducedRiceSales_pt", new[] { "Party_Id" });
            DropIndex("dbo.ProducedRiceSales_ch", new[] { "prsp_id" });
            DropIndex("dbo.ProducedRiceSales_ch", new[] { "Rice_Production_id" });
            DropTable("dbo.ProducedRiceSales_pt");
            DropTable("dbo.ProducedRiceSales_ch");
        }
    }
}
