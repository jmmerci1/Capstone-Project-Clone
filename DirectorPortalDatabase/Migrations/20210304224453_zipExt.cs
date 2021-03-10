using Microsoft.EntityFrameworkCore.Migrations;

namespace DirectorPortalDatabase.Migrations
{
    public partial class zipExt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "zipExt",
                table: "Addresses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "zipExt",
                table: "Addresses");
        }
    }
}
