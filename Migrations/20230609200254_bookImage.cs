using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteka.Migrations
{
    /// <inheritdoc />
    public partial class bookImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookImagesBookImageId",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BooksImage",
                columns: table => new
                {
                    BookImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BooksImage", x => x.BookImageId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_BookImagesBookImageId",
                table: "Books",
                column: "BookImagesBookImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BooksImage_BookImagesBookImageId",
                table: "Books",
                column: "BookImagesBookImageId",
                principalTable: "BooksImage",
                principalColumn: "BookImageId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_BooksImage_BookImagesBookImageId",
                table: "Books");

            migrationBuilder.DropTable(
                name: "BooksImage");

            migrationBuilder.DropIndex(
                name: "IX_Books_BookImagesBookImageId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BookImagesBookImageId",
                table: "Books");
        }
    }
}
