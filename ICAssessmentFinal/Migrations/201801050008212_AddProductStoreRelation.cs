namespace ICAssessmentFinal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProductStoreRelation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StoreProducts",
                c => new
                    {
                        Store_Id = c.Int(nullable: false),
                        Product_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Store_Id, t.Product_Id })
                .ForeignKey("dbo.Stores", t => t.Store_Id, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .Index(t => t.Store_Id)
                .Index(t => t.Product_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StoreProducts", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.StoreProducts", "Store_Id", "dbo.Stores");
            DropIndex("dbo.StoreProducts", new[] { "Product_Id" });
            DropIndex("dbo.StoreProducts", new[] { "Store_Id" });
            DropTable("dbo.StoreProducts");
        }
    }
}
