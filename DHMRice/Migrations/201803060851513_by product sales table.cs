namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class byproductsalestable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BpRiceSales_ch",
                c => new
                    {
                        bprsc_id = c.Int(nullable: false, identity: true),
                        bprsc_title = c.String(),
                        Rice_Production_id = c.Int(nullable: false),
                        bprsc_qty = c.Int(nullable: false),
                        bprsc_Weight_kg = c.Decimal(nullable: false, precision: 18, scale: 2),
                        bprsc_Weight_mann = c.Decimal(nullable: false, precision: 18, scale: 2),
                        bprsc_price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        bprsp_id = c.Int(nullable: false),
                        bprsc_status = c.Boolean(nullable: false),
                        ProducedRiceSales_pt_prsp_id = c.Int(),
                    })
                .PrimaryKey(t => t.bprsc_id)
                .ForeignKey("dbo.ProducedRiceSales_pt", t => t.ProducedRiceSales_pt_prsp_id)
                .ForeignKey("dbo.Rice_Production", t => t.Rice_Production_id, cascadeDelete: true)
                .Index(t => t.Rice_Production_id)
                .Index(t => t.ProducedRiceSales_pt_prsp_id);
            
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
                .ForeignKey("dbo.Parties", t => t.Party_Id, cascadeDelete: true)
                .Index(t => t.Party_Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BpRiceSales_pt", "Party_Id", "dbo.Parties");
            DropForeignKey("dbo.BpRiceSales_pt", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.BpRiceSales_ch", "Rice_Production_id", "dbo.Rice_Production");
            DropForeignKey("dbo.BpRiceSales_ch", "ProducedRiceSales_pt_prsp_id", "dbo.ProducedRiceSales_pt");
            DropIndex("dbo.BpRiceSales_pt", new[] { "Id" });
            DropIndex("dbo.BpRiceSales_pt", new[] { "Party_Id" });
            DropIndex("dbo.BpRiceSales_ch", new[] { "ProducedRiceSales_pt_prsp_id" });
            DropIndex("dbo.BpRiceSales_ch", new[] { "Rice_Production_id" });
            DropTable("dbo.BpRiceSales_pt");
            DropTable("dbo.BpRiceSales_ch");
        }
    }
}
