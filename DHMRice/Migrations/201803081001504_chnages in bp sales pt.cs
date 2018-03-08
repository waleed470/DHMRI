namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chnagesinbpsalespt : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BpRiceSales_ch", "ProducedRiceSales_pt_prsp_id", "dbo.ProducedRiceSales_pt");
            DropIndex("dbo.BpRiceSales_ch", new[] { "ProducedRiceSales_pt_prsp_id" });
            CreateIndex("dbo.BpRiceSales_ch", "bprsp_id");
            AddForeignKey("dbo.BpRiceSales_ch", "bprsp_id", "dbo.BpRiceSales_pt", "bprsp_id", cascadeDelete: true);
            DropColumn("dbo.BpRiceSales_ch", "ProducedRiceSales_pt_prsp_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BpRiceSales_ch", "ProducedRiceSales_pt_prsp_id", c => c.Int());
            DropForeignKey("dbo.BpRiceSales_ch", "bprsp_id", "dbo.BpRiceSales_pt");
            DropIndex("dbo.BpRiceSales_ch", new[] { "bprsp_id" });
            CreateIndex("dbo.BpRiceSales_ch", "ProducedRiceSales_pt_prsp_id");
            AddForeignKey("dbo.BpRiceSales_ch", "ProducedRiceSales_pt_prsp_id", "dbo.ProducedRiceSales_pt", "prsp_id");
        }
    }
}
