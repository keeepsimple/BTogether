﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTogether.Data.Migrations
{
    public partial class EditLoveModel4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayText",
                table: "Loves",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayText",
                table: "Loves");
        }
    }
}
