using Microsoft.EntityFrameworkCore.Migrations;

namespace DirectorPortalDatabase.Migrations
{
    public partial class RemoveRedundantColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "zip",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "city",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "state",
                table: "Businesses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "zip",
                table: "Businesses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "Businesses",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "state",
                table: "Businesses",
                type: "TEXT",
                nullable: true);
        }
    }
}
