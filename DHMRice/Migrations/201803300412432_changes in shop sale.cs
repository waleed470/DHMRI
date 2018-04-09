namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesinshopsale : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ShopStocks", "Rice_Production_id", "dbo.Rice_Production");
            DropForeignKey("dbo.ShopStocks", "srsp_id", "dbo.ShopStock_pt");
            DropIndex("dbo.ShopStocks", new[] { "Rice_Production_id" });
            DropIndex("dbo.ShopStocks", new[] { "srsp_id" });
            AlterColumn("dbo.ShopStocks", "Rice_Production_id", c => c.Int());
            AlterColumn("dbo.ShopStocks", "srsp_id", c => c.Int());
            CreateIndex("dbo.ShopStocks", "Rice_Production_id");
            CreateIndex("dbo.ShopStocks", "srsp_id");
            AddForeignKey("dbo.ShopStocks", "Rice_Production_id", "dbo.Rice_Production", "Rice_Production_id");
            AddForeignKey("dbo.ShopStocks", "srsp_id", "dbo.ShopStock_pt", "srsp_id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShopStocks", "srsp_id", "dbo.ShopStock_pt");
            DropForeignKey("dbo.ShopStocks", "Rice_Production_id", "dbo.Rice_Production");
            DropIndex("dbo.ShopStocks", new[] { "srsp_id" });
            DropIndex("dbo.ShopStocks", new[] { "Rice_Production_id" });
            AlterColumn("dbo.ShopStocks", "srsp_id", c => c.Int(nullable: false));
            AlterColumn("dbo.ShopStocks", "Rice_Production_id", c => c.Int(nullable: false));
            CreateIndex("dbo.ShopStocks", "srsp_id");
            CreateIndex("dbo.ShopStocks", "Rice_Production_id");
            AddForeignKey("dbo.ShopStocks", "srsp_id", "dbo.ShopStock_pt", "srsp_id", cascadeDelete: true);
            AddForeignKey("dbo.ShopStocks", "Rice_Production_id", "dbo.Rice_Production", "Rice_Production_id", cascadeDelete: true);
        }
    }
}
