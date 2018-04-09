namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chagesinrawricetable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RawRices", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.RawRices", "Broker_Id", "dbo.Brokers");
            DropForeignKey("dbo.RawRices", "Party_Id", "dbo.Parties");
            DropIndex("dbo.RawRices", new[] { "Party_Id" });
            DropIndex("dbo.RawRices", new[] { "Id" });
            DropIndex("dbo.RawRices", new[] { "Broker_Id" });
            AlterColumn("dbo.RawRices", "Party_Id", c => c.Int());
            AlterColumn("dbo.RawRices", "Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.RawRices", "Broker_Id", c => c.Int());
            AlterColumn("dbo.RawRices", "Item_Name", c => c.String());
            AlterColumn("dbo.RawRices", "Item_Code", c => c.String());
            CreateIndex("dbo.RawRices", "Party_Id");
            CreateIndex("dbo.RawRices", "Id");
            CreateIndex("dbo.RawRices", "Broker_Id");
            AddForeignKey("dbo.RawRices", "Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.RawRices", "Broker_Id", "dbo.Brokers", "Broker_Id");
            AddForeignKey("dbo.RawRices", "Party_Id", "dbo.Parties", "Party_Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RawRices", "Party_Id", "dbo.Parties");
            DropForeignKey("dbo.RawRices", "Broker_Id", "dbo.Brokers");
            DropForeignKey("dbo.RawRices", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.RawRices", new[] { "Broker_Id" });
            DropIndex("dbo.RawRices", new[] { "Id" });
            DropIndex("dbo.RawRices", new[] { "Party_Id" });
            AlterColumn("dbo.RawRices", "Item_Code", c => c.String(nullable: false));
            AlterColumn("dbo.RawRices", "Item_Name", c => c.String(nullable: false));
            AlterColumn("dbo.RawRices", "Broker_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.RawRices", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.RawRices", "Party_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.RawRices", "Broker_Id");
            CreateIndex("dbo.RawRices", "Id");
            CreateIndex("dbo.RawRices", "Party_Id");
            AddForeignKey("dbo.RawRices", "Party_Id", "dbo.Parties", "Party_Id", cascadeDelete: true);
            AddForeignKey("dbo.RawRices", "Broker_Id", "dbo.Brokers", "Broker_Id", cascadeDelete: true);
            AddForeignKey("dbo.RawRices", "Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
