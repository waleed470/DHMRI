namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GatePassOutwards", "RiceType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GatePassOutwards", "RiceType");
        }
    }
}
