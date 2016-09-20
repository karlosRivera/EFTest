namespace EFTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsDefault : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "IsDefault", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contacts", "IsDefault");
        }
    }
}
