using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsWebAPI.Migrations
{
    public partial class initdb2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SavedAt",
                table: "SavedArticles",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SavedAt",
                table: "SavedArticles");
        }
    }
}
