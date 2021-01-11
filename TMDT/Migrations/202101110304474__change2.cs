namespace TMDT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _change2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Books", "Comment_CommentId", "dbo.Comments");
            RenameColumn(table: "dbo.Books", name: "Comment_CommentId", newName: "Comment_BookID");
            RenameIndex(table: "dbo.Books", name: "IX_Comment_CommentId", newName: "IX_Comment_BookID");
            DropPrimaryKey("dbo.Comments");
            AlterColumn("dbo.Comments", "CommentId", c => c.Int(nullable: false));
            AlterColumn("dbo.Comments", "BookID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Comments", "BookID");
            AddForeignKey("dbo.Books", "Comment_BookID", "dbo.Comments", "BookID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "Comment_BookID", "dbo.Comments");
            DropPrimaryKey("dbo.Comments");
            AlterColumn("dbo.Comments", "BookID", c => c.Int(nullable: false));
            AlterColumn("dbo.Comments", "CommentId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Comments", "CommentId");
            RenameIndex(table: "dbo.Books", name: "IX_Comment_BookID", newName: "IX_Comment_CommentId");
            RenameColumn(table: "dbo.Books", name: "Comment_BookID", newName: "Comment_CommentId");
            AddForeignKey("dbo.Books", "Comment_CommentId", "dbo.Comments", "CommentId");
        }
    }
}
