using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsWebAPI.Migrations
{
    public partial class updateDB11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContentIndex",
                table: "Contents",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentIndex",
                table: "Contents");
        }
    }
}
