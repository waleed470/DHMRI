namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gatepassoutward : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GatePassOutwards",
                c => new
                    {
                        GatePassOutwardId = c.Int(nullable: false, identity: true),
                        RiceTypeId = c.Int(nullable: false),
                        Vehicle_No = c.String(),
                        Driver_Name = c.String(),
                        Bility_No = c.Int(nullable: false),
                        Designation = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.GatePassOutwardId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GatePassOutwards");
        }
    }
}
