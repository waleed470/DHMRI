namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletecommisoncolums : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.RawRices", "BrokerCommissionPercentage");
            DropColumn("dbo.RawRices", "BrokerCommissionAmount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RawRices", "BrokerCommissionAmount", c => c.Int(nullable: false));
            AddColumn("dbo.RawRices", "BrokerCommissionPercentage", c => c.Int(nullable: false));
        }
    }
}
