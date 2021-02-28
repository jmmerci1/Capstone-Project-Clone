using Microsoft.EntityFrameworkCore.Migrations;

namespace DirectorPortalDatabase.Migrations
{
    public partial class InitialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    address = table.Column<string>(nullable: true),
                    city = table.Column<string>(nullable: true),
                    state = table.Column<string>(nullable: true),
                    zip = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Businesses",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(nullable: true),
                    established = table.Column<int>(nullable: false),
                    level = table.Column<int>(nullable: false),
                    mailingAddressId = table.Column<int>(nullable: false),
                    physicalAddressId = table.Column<int>(nullable: false),
                    website = table.Column<string>(nullable: true),
                    notes = table.Column<string>(nullable: true),
                    extraFields = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Businesses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ContactPeople",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactPeople", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PhoneNumbers",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    contactPersonId = table.Column<int>(nullable: false),
                    phoneNumber = table.Column<string>(nullable: true),
                    notes = table.Column<string>(nullable: true),
                    type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneNumbers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TodoListItems",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    title = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    complete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoListItems", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "BusinessYearlyData",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    year = table.Column<int>(nullable: false),
                    businessId = table.Column<int>(nullable: false),
                    duesPaid = table.Column<double>(nullable: false),
                    raffleTicketsReturned = table.Column<double>(nullable: false),
                    credit = table.Column<double>(nullable: false),
                    terms = table.Column<int>(nullable: false),
                    ballotNum = table.Column<int>(nullable: false),
                    extraFields = table.Column<string>(nullable: true),
                    BusinessGIntId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessYearlyData", x => x.id);
                    table.ForeignKey(
                        name: "FK_BusinessYearlyData_Businesses_BusinessGIntId",
                        column: x => x.BusinessGIntId,
                        principalTable: "Businesses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BusinessReps",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    businessId = table.Column<int>(nullable: false),
                    contactPersonId = table.Column<int>(nullable: false),
                    ContactPersonGIntId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessReps", x => x.id);
                    table.ForeignKey(
                        name: "FK_BusinessReps_ContactPeople_ContactPersonGIntId",
                        column: x => x.ContactPersonGIntId,
                        principalTable: "ContactPeople",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Emails",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    contactPersonId = table.Column<int>(nullable: false),
                    email = table.Column<string>(nullable: true),
                    ContactPersonGIntId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emails", x => x.id);
                    table.ForeignKey(
                        name: "FK_Emails_ContactPeople_ContactPersonGIntId",
                        column: x => x.ContactPersonGIntId,
                        principalTable: "ContactPeople",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessReps_ContactPersonGIntId",
                table: "BusinessReps",
                column: "ContactPersonGIntId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessYearlyData_BusinessGIntId",
                table: "BusinessYearlyData",
                column: "BusinessGIntId");

            migrationBuilder.CreateIndex(
                name: "IX_Emails_ContactPersonGIntId",
                table: "Emails",
                column: "ContactPersonGIntId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "BusinessReps");

            migrationBuilder.DropTable(
                name: "BusinessYearlyData");

            migrationBuilder.DropTable(
                name: "Emails");

            migrationBuilder.DropTable(
                name: "PhoneNumbers");

            migrationBuilder.DropTable(
                name: "TodoListItems");

            migrationBuilder.DropTable(
                name: "Businesses");

            migrationBuilder.DropTable(
                name: "ContactPeople");
        }
    }
}
