using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTogether.Data.Migrations
{
    public partial class UpdateModelUser3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayText",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeImage",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayText",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HomeImage",
                table: "Users");
        }
    }
}
