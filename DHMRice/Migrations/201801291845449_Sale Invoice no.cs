namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SaleInvoiceno : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SaleInvoices",
                c => new
                    {
                        SaleInvoice_id = c.Int(nullable: false, identity: true),
                        SaleInvoice_no = c.Int(nullable: false),
                        Sale_id = c.Int(nullable: false),
                        SaleInvoice_type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SaleInvoice_id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SaleInvoices");
        }
    }
}
