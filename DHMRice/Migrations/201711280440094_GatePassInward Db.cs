namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GatePassInwardDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GatePassInwareds",
                c => new
                    {
                        GatePassInwaredId = c.Int(nullable: false, identity: true),
                        RawRice_id = c.Int(nullable: false),
                        Vehicle_No = c.String(),
                        Driver_Name = c.String(),
                        Bility_No = c.Int(nullable: false),
                        Purchased_By = c.String(),
                        Designation = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.GatePassInwaredId)
                .ForeignKey("dbo.RawRices", t => t.RawRice_id, cascadeDelete: true)
                .Index(t => t.RawRice_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GatePassInwareds", "RawRice_id", "dbo.RawRices");
            DropIndex("dbo.GatePassInwareds", new[] { "RawRice_id" });
            DropTable("dbo.GatePassInwareds");
        }
    }
}
