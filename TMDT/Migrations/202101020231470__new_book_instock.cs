namespace TMDT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _new_book_instock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "InStock", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "InStock");
        }
    }
}
