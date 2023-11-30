using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsWebAPI.Migrations
{
    public partial class initdb3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SavedArticles_ArticleID",
                table: "SavedArticles");

            migrationBuilder.DropColumn(
                name: "SaveArticleID",
                table: "Articles");

            migrationBuilder.CreateIndex(
                name: "IX_SavedArticles_ArticleID",
                table: "SavedArticles",
                column: "ArticleID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SavedArticles_ArticleID",
                table: "SavedArticles");

            migrationBuilder.AddColumn<int>(
                name: "SaveArticleID",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SavedArticles_ArticleID",
                table: "SavedArticles",
                column: "ArticleID",
                unique: true);
        }
    }
}
