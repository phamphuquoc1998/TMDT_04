namespace TMDT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _chang : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "BookID", "dbo.Books");
            DropIndex("dbo.Comments", new[] { "BookID" });
            CreateTable(
                "dbo.Prices",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        price = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Books", "InStock", c => c.Int(nullable: false));
            AddColumn("dbo.Books", "Comment_CommentId", c => c.Int());
            AddColumn("dbo.Comments", "Name", c => c.String());
            AddColumn("dbo.Comments", "Avatar", c => c.String());
            AddColumn("dbo.Orders", "NameRec", c => c.String());
            AddColumn("dbo.Orders", "AddressOrder", c => c.String(nullable: false));
            AddColumn("dbo.Orders", "PhoneOrder", c => c.String(nullable: false));
            AddColumn("dbo.Orders", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "Payment", c => c.String());
            CreateIndex("dbo.Books", "Comment_CommentId");
            AddForeignKey("dbo.Books", "Comment_CommentId", "dbo.Comments", "CommentId");
            DropColumn("dbo.Orders", "SubTotal");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "SubTotal", c => c.Single());
            DropForeignKey("dbo.Books", "Comment_CommentId", "dbo.Comments");
            DropIndex("dbo.Books", new[] { "Comment_CommentId" });
            DropColumn("dbo.Orders", "Payment");
            DropColumn("dbo.Orders", "Status");
            DropColumn("dbo.Orders", "PhoneOrder");
            DropColumn("dbo.Orders", "AddressOrder");
            DropColumn("dbo.Orders", "NameRec");
            DropColumn("dbo.Comments", "Avatar");
            DropColumn("dbo.Comments", "Name");
            DropColumn("dbo.Books", "Comment_CommentId");
            DropColumn("dbo.Books", "InStock");
            DropTable("dbo.Prices");
            CreateIndex("dbo.Comments", "BookID");
            AddForeignKey("dbo.Comments", "BookID", "dbo.Books", "BookID", cascadeDelete: true);
        }
    }
}
