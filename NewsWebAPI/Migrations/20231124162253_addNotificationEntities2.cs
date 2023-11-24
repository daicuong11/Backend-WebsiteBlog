using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsWebAPI.Migrations
{
    public partial class addNotificationEntities2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Loves",
                columns: table => new
                {
                    LoveID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserTargetID = table.Column<int>(type: "int", nullable: false),
                    ArticleTargetID = table.Column<int>(type: "int", nullable: false),
                    ArticleID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loves", x => x.LoveID);
                    table.ForeignKey(
                        name: "FK_Loves_Articles_ArticleID",
                        column: x => x.ArticleID,
                        principalTable: "Articles",
                        principalColumn: "ArticleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Loves_ArticleID",
                table: "Loves",
                column: "ArticleID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Loves");
        }
    }
}
