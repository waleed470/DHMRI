namespace DHMRice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newmig : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PartyRemainings",
                c => new
                    {
                        PartyRemaining_Id = c.Int(nullable: false, identity: true),
                        Party_Id = c.Int(nullable: false),
                        RemainingType = c.String(),
                        isPayed = c.Boolean(nullable: false),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RemainingAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.PartyRemaining_Id)
                .ForeignKey("dbo.Parties", t => t.Party_Id, cascadeDelete: true)
                .Index(t => t.Party_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PartyRemainings", "Party_Id", "dbo.Parties");
            DropIndex("dbo.PartyRemainings", new[] { "Party_Id" });
            DropTable("dbo.PartyRemainings");
        }
    }
}
