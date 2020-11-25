namespace TMDT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit_new_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        BookID = c.Int(nullable: false),
                        Content = c.String(),
                        Time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Books", t => t.BookID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.BookID);
            
            CreateTable(
                "dbo.Vouchers",
                c => new
                    {
                        VoucherId = c.Int(nullable: false, identity: true),
                        VoucherName = c.String(),
                        VoucherValue = c.String(),
                        VoucherDetails = c.String(),
                    })
                .PrimaryKey(t => t.VoucherId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "BookID", "dbo.Books");
            DropIndex("dbo.Comments", new[] { "BookID" });
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropTable("dbo.Vouchers");
            DropTable("dbo.Comments");
        }
    }
}
