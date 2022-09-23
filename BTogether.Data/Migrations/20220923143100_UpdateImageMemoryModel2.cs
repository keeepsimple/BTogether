using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTogether.Data.Migrations
{
    public partial class UpdateImageMemoryModel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ImageMemories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ImageMemories");
        }
    }
}
