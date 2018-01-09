namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newmig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RawRice_Sales_ch", "rsc_title", c => c.String());
            AddColumn("dbo.RawRice_Sales_pt", "rsp_Title", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RawRice_Sales_pt", "rsp_Title");
            DropColumn("dbo.RawRice_Sales_ch", "rsc_title");
        }
    }
}
