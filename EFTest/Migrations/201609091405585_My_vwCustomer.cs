namespace EFTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class My_vwCustomer : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.vwCustomers",
            //    c => new
            //        {
            //            CustomerID = c.Int(nullable: false, identity: true),
            //            FirstName = c.String(),
            //        })
            //    .PrimaryKey(t => t.CustomerID);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.vwCustomers");
        }
    }
}
