namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeDatatype : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SaleInvoices", "SaleInvoice_type", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SaleInvoices", "SaleInvoice_type", c => c.Int(nullable: false));
        }
    }
}
