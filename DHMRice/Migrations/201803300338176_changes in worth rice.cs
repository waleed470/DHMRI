namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesinworthrice : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Rice_Production_ProductWorth", "Rice_Produce_Bags_id", "dbo.Rice_Produce_Bag");
            DropIndex("dbo.Rice_Production_ProductWorth", new[] { "Rice_Produce_Bags_id" });
            AlterColumn("dbo.Rice_Production_ProductWorth", "Rice_Produce_Bags_id", c => c.Int());
            CreateIndex("dbo.Rice_Production_ProductWorth", "Rice_Produce_Bags_id");
            AddForeignKey("dbo.Rice_Production_ProductWorth", "Rice_Produce_Bags_id", "dbo.Rice_Produce_Bag", "Rice_Produce_Bags_id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rice_Production_ProductWorth", "Rice_Produce_Bags_id", "dbo.Rice_Produce_Bag");
            DropIndex("dbo.Rice_Production_ProductWorth", new[] { "Rice_Produce_Bags_id" });
            AlterColumn("dbo.Rice_Production_ProductWorth", "Rice_Produce_Bags_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Rice_Production_ProductWorth", "Rice_Produce_Bags_id");
            AddForeignKey("dbo.Rice_Production_ProductWorth", "Rice_Produce_Bags_id", "dbo.Rice_Produce_Bag", "Rice_Produce_Bags_id", cascadeDelete: true);
        }
    }
}
