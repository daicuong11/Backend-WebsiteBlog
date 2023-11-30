using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsWebAPI.Migrations
{
    public partial class initdb1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SaveArticleID",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SavedArticles",
                columns: table => new
                {
                    SavedArticleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserTargetID = table.Column<int>(type: "int", nullable: false),
                    ArticleID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedArticles", x => x.SavedArticleID);
                    table.ForeignKey(
                        name: "FK_SavedArticles_Articles_ArticleID",
                        column: x => x.ArticleID,
                        principalTable: "Articles",
                        principalColumn: "ArticleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SavedArticles_ArticleID",
                table: "SavedArticles",
                column: "ArticleID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SavedArticles");

            migrationBuilder.DropColumn(
                name: "SaveArticleID",
                table: "Articles");
        }
    }
}
