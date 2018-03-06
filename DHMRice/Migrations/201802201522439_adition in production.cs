namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aditioninproduction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProducedRiceSales_pt", "Carriage", c => c.Int(nullable: false));
            AddColumn("dbo.ProducedRiceSales_pt", "Labour", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProducedRiceSales_pt", "Labour");
            DropColumn("dbo.ProducedRiceSales_pt", "Carriage");
        }
    }
}
