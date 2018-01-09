namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _25112017mig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RawRice_Sales_ch", "rsc_status", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RawRice_Sales_ch", "rsc_status");
        }
    }
}
