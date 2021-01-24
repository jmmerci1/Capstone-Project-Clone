namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Student", "Address", c => c.String(maxLength: 2147483647));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Student", "Address");
        }
    }
}
