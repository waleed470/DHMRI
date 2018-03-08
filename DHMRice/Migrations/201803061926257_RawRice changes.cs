namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RawRicechanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RawRices", "Pay_CommissionPercentage", c => c.Boolean(nullable: false));
            AddColumn("dbo.RawRices", "BrokerCommissionPercentage", c => c.Int(nullable: false));
            AddColumn("dbo.RawRices", "BrokerCommissionAmount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RawRices", "BrokerCommissionAmount");
            DropColumn("dbo.RawRices", "BrokerCommissionPercentage");
            DropColumn("dbo.RawRices", "Pay_CommissionPercentage");
        }
    }
}
