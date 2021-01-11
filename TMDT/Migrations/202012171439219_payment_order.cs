namespace TMDT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class payment_order : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "Payment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Payment");
            DropColumn("dbo.Orders", "Status");
        }
    }
}
