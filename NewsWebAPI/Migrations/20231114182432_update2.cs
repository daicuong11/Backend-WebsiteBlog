using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsWebAPI.Migrations
{
    public partial class update2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contents_ArticleID",
                table: "Contents");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_ArticleID",
                table: "Contents",
                column: "ArticleID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contents_ArticleID",
                table: "Contents");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_ArticleID",
                table: "Contents",
                column: "ArticleID",
                unique: true);
        }
    }
}
