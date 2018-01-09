namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class REmoverequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Brokers", "Broker_Code", c => c.String());
            AlterColumn("dbo.Brokers", "Broker_MobileNo", c => c.String());
            AlterColumn("dbo.Brokers", "Broker_BankName", c => c.String());
            AlterColumn("dbo.Brokers", "Broker_ACcountNo", c => c.String());
            AlterColumn("dbo.Brokers", "Broker_Address", c => c.String());
            AlterColumn("dbo.Factories", "Factory_Code", c => c.String());
            AlterColumn("dbo.Parties", "Party_Code", c => c.String());
            AlterColumn("dbo.Parties", "Party_BankName", c => c.String());
            AlterColumn("dbo.Parties", "Party_ACcountNo", c => c.String());
            AlterColumn("dbo.Parties", "Party_Address", c => c.String());
            AlterColumn("dbo.Shops", "Shop_Code", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Shops", "Shop_Code", c => c.String(nullable: false));
            AlterColumn("dbo.Parties", "Party_Address", c => c.String(nullable: false));
            AlterColumn("dbo.Parties", "Party_ACcountNo", c => c.String(nullable: false));
            AlterColumn("dbo.Parties", "Party_BankName", c => c.String(nullable: false));
            AlterColumn("dbo.Parties", "Party_Code", c => c.String(nullable: false));
            AlterColumn("dbo.Factories", "Factory_Code", c => c.String(nullable: false));
            AlterColumn("dbo.Brokers", "Broker_Address", c => c.String(nullable: false));
            AlterColumn("dbo.Brokers", "Broker_ACcountNo", c => c.String(nullable: false));
            AlterColumn("dbo.Brokers", "Broker_BankName", c => c.String(nullable: false));
            AlterColumn("dbo.Brokers", "Broker_MobileNo", c => c.String(nullable: false));
            AlterColumn("dbo.Brokers", "Broker_Code", c => c.String(nullable: false));
        }
    }
}
