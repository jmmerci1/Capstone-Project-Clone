namespace DirectorPortalDatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDatabaseSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Address",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        address = c.String(),
                        city = c.String(),
                        state = c.String(),
                        zip = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Business",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        established = c.Int(nullable: false),
                        level = c.Int(nullable: false),
                        mailingAddressId = c.Int(nullable: false),
                        physicalAddressId = c.Int(nullable: false),
                        city = c.String(),
                        state = c.String(),
                        zip = c.Int(nullable: false),
                        website = c.String(),
                        notes = c.String(),
                        extraFields = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.YearlyData",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        year = c.Int(nullable: false),
                        businessId = c.Int(nullable: false),
                        duesPaid = c.Double(nullable: false),
                        raffleTicketsReturned = c.Double(nullable: false),
                        credit = c.Double(nullable: false),
                        terms = c.Int(nullable: false),
                        ballotNum = c.Int(nullable: false),
                        extraFields = c.String(),
                        Business_GIntId = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Business", t => t.Business_GIntId)
                .Index(t => t.Business_GIntId);
            
            CreateTable(
                "dbo.BusinessRep",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        businessId = c.Int(nullable: false),
                        contactPersonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ContactPerson",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Email",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        contactPersonId = c.Int(nullable: false),
                        email = c.String(),
                        ContactPerson_GIntId = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.ContactPerson", t => t.ContactPerson_GIntId)
                .Index(t => t.ContactPerson_GIntId);
            
            CreateTable(
                "dbo.PhoneNumber",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        contactPersonId = c.Int(nullable: false),
                        phoneNumber = c.String(),
                        notes = c.String(),
                        type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Email", "ContactPerson_GIntId", "dbo.ContactPerson");
            DropForeignKey("dbo.YearlyData", "Business_GIntId", "dbo.Business");
            DropIndex("dbo.Email", new[] { "ContactPerson_GIntId" });
            DropIndex("dbo.YearlyData", new[] { "Business_GIntId" });
            DropTable("dbo.PhoneNumber");
            DropTable("dbo.Email");
            DropTable("dbo.ContactPerson");
            DropTable("dbo.BusinessRep");
            DropTable("dbo.YearlyData");
            DropTable("dbo.Business");
            DropTable("dbo.Address");
        }
    }
}
