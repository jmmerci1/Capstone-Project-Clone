using Microsoft.EntityFrameworkCore.Migrations;

namespace DirectorPortalDatabase.Migrations
{
    public partial class CategoryMigrate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    category = table.Column<string>(nullable: true),
                    BusinessGIntId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.id);
                    table.ForeignKey(
                        name: "FK_Categories_Businesses_BusinessGIntId",
                        column: x => x.BusinessGIntId,
                        principalTable: "Businesses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CategoryRef",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    businessId = table.Column<int>(nullable: false),
                    categoriesID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryRef", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_BusinessGIntId",
                table: "Categories",
                column: "BusinessGIntId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "CategoryRef");
        }
    }
}
