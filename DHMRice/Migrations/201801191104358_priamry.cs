namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class priamry : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ShopStocks", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ShopStocks", new[] { "ApplicationUser_Id" });
            DropPrimaryKey("dbo.ShopStocks");
            AlterColumn("dbo.ShopStocks", "ShopStockId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.ShopStocks", "ShopStockId");
            DropColumn("dbo.ShopStocks", "Id");
            DropColumn("dbo.ShopStocks", "ApplicationUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ShopStocks", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.ShopStocks", "Id", c => c.String(nullable: false, maxLength: 128));
            DropPrimaryKey("dbo.ShopStocks");
            AlterColumn("dbo.ShopStocks", "ShopStockId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.ShopStocks", "Id");
            CreateIndex("dbo.ShopStocks", "ApplicationUser_Id");
            AddForeignKey("dbo.ShopStocks", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
