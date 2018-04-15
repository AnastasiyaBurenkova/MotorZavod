namespace AbstractShopService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Detalis",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DetaliName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DvigateliDetalis",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DvigateliId = c.Int(nullable: false),
                        DetaliId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Detalis", t => t.DetaliId, cascadeDelete: true)
                .ForeignKey("dbo.Dvigatelis", t => t.DvigateliId, cascadeDelete: true)
                .Index(t => t.DvigateliId)
                .Index(t => t.DetaliId);
            
            CreateTable(
                "dbo.Dvigatelis",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DvigateliName = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Zakazs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ZakazchikId = c.Int(nullable: false),
                        DvigateliId = c.Int(nullable: false),
                        RabochiId = c.Int(),
                        Count = c.Int(nullable: false),
                        Sum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        DateCreate = c.DateTime(nullable: false),
                        DateImplement = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Dvigatelis", t => t.DvigateliId, cascadeDelete: true)
                .ForeignKey("dbo.Rabochis", t => t.RabochiId)
                .ForeignKey("dbo.Zakazchiks", t => t.ZakazchikId, cascadeDelete: true)
                .Index(t => t.ZakazchikId)
                .Index(t => t.DvigateliId)
                .Index(t => t.RabochiId);
            
            CreateTable(
                "dbo.Rabochis",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RabochiFIO = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Zakazchiks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ZakazchikFIO = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GarazhDetalis",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GarazhId = c.Int(nullable: false),
                        DetaliId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Detalis", t => t.DetaliId, cascadeDelete: true)
                .ForeignKey("dbo.Garazhs", t => t.GarazhId, cascadeDelete: true)
                .Index(t => t.GarazhId)
                .Index(t => t.DetaliId);
            
            CreateTable(
                "dbo.Garazhs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GarazhName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GarazhDetalis", "GarazhId", "dbo.Garazhs");
            DropForeignKey("dbo.GarazhDetalis", "DetaliId", "dbo.Detalis");
            DropForeignKey("dbo.Zakazs", "ZakazchikId", "dbo.Zakazchiks");
            DropForeignKey("dbo.Zakazs", "RabochiId", "dbo.Rabochis");
            DropForeignKey("dbo.Zakazs", "DvigateliId", "dbo.Dvigatelis");
            DropForeignKey("dbo.DvigateliDetalis", "DvigateliId", "dbo.Dvigatelis");
            DropForeignKey("dbo.DvigateliDetalis", "DetaliId", "dbo.Detalis");
            DropIndex("dbo.GarazhDetalis", new[] { "DetaliId" });
            DropIndex("dbo.GarazhDetalis", new[] { "GarazhId" });
            DropIndex("dbo.Zakazs", new[] { "RabochiId" });
            DropIndex("dbo.Zakazs", new[] { "DvigateliId" });
            DropIndex("dbo.Zakazs", new[] { "ZakazchikId" });
            DropIndex("dbo.DvigateliDetalis", new[] { "DetaliId" });
            DropIndex("dbo.DvigateliDetalis", new[] { "DvigateliId" });
            DropTable("dbo.Garazhs");
            DropTable("dbo.GarazhDetalis");
            DropTable("dbo.Zakazchiks");
            DropTable("dbo.Rabochis");
            DropTable("dbo.Zakazs");
            DropTable("dbo.Dvigatelis");
            DropTable("dbo.DvigateliDetalis");
            DropTable("dbo.Detalis");
        }
    }
}
