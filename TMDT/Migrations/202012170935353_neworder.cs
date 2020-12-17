namespace TMDT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class neworder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "NameRec", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "NameRec");
        }
    }
}
