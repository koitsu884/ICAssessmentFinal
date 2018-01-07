namespace ICAssessmentFinal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomFields : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Double(nullable: false),
                        Category = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductSolds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateSold = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Customer_Id = c.Int(),
                        Product_Id = c.Int(),
                        Store_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .ForeignKey("dbo.Products", t => t.Product_Id)
                .ForeignKey("dbo.Stores", t => t.Store_Id)
                .Index(t => t.Customer_Id)
                .Index(t => t.Product_Id)
                .Index(t => t.Store_Id);
            
            CreateTable(
                "dbo.Stores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductSolds", "Store_Id", "dbo.Stores");
            DropForeignKey("dbo.ProductSolds", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.ProductSolds", "Customer_Id", "dbo.Customers");
            DropIndex("dbo.ProductSolds", new[] { "Store_Id" });
            DropIndex("dbo.ProductSolds", new[] { "Product_Id" });
            DropIndex("dbo.ProductSolds", new[] { "Customer_Id" });
            DropTable("dbo.Stores");
            DropTable("dbo.ProductSolds");
            DropTable("dbo.Products");
        }
    }
}
