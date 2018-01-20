namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class useridinshopstock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShopStocks", "Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.ShopStocks", "Id");
            AddForeignKey("dbo.ShopStocks", "Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShopStocks", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.ShopStocks", new[] { "Id" });
            DropColumn("dbo.ShopStocks", "Id");
        }
    }
}
