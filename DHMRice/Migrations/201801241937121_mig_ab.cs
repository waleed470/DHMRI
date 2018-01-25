namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig_ab : DbMigration
    {
        public override void Up()
        {
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
            DropForeignKey("dbo.Opening_ClosingDays_Shop", "Shop_Id", "dbo.Shops");
            DropIndex("dbo.Transaction_Shop", new[] { "Opening_ClosingDays_Shop_id" });
            DropIndex("dbo.Opening_ClosingDays_Shop", new[] { "Shop_Id" });
            DropTable("dbo.Transaction_Shop");
            DropTable("dbo.Opening_ClosingDays_Shop");
        }
    }
}
