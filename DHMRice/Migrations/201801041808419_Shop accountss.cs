namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Shopaccountss : DbMigration
    {
        public override void Up()
        {
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
            
            AlterColumn("dbo.Shops", "Shop_MobileNo", c => c.String());
            AlterColumn("dbo.Shops", "Shop_Address", c => c.String());
            DropColumn("dbo.Shops", "Shop_BankName");
            DropColumn("dbo.Shops", "Shop_ACcountNo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Shops", "Shop_ACcountNo", c => c.String(nullable: false));
            AddColumn("dbo.Shops", "Shop_BankName", c => c.String(nullable: false));
            DropForeignKey("dbo.Shop_Account", "Shop_Id", "dbo.Shops");
            DropIndex("dbo.Shop_Account", new[] { "Shop_Id" });
            AlterColumn("dbo.Shops", "Shop_Address", c => c.String(nullable: false));
            AlterColumn("dbo.Shops", "Shop_MobileNo", c => c.String(nullable: false));
            DropTable("dbo.Shop_Account");
        }
    }
}
