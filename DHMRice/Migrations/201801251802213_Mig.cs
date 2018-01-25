namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customers", "Customer_BankName", c => c.String());
            AlterColumn("dbo.Customers", "Customer_ACcountNo", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customers", "Customer_ACcountNo", c => c.String(nullable: false));
            AlterColumn("dbo.Customers", "Customer_BankName", c => c.String(nullable: false));
        }
    }
}
