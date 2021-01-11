namespace TMDT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _chang4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Prices",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        price = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Prices");
        }
    }
}
