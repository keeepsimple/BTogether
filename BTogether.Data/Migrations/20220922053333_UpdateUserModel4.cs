using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTogether.Data.Migrations
{
    public partial class UpdateUserModel4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInLoveWith",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IsInLoveWith",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
