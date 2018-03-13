namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcommisioncolum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RawRices", "BrokerCommissionPercentage", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.RawRices", "BrokerCommissionAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RawRices", "BrokerCommissionAmount");
            DropColumn("dbo.RawRices", "BrokerCommissionPercentage");
        }
    }
}
