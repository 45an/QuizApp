using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizApp.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class QuizMedia2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Media_QuizMediaId",
                table: "Quizzes");

            migrationBuilder.RenameColumn(
                name: "QuizMediaId",
                table: "Quizzes",
                newName: "MediaId");

            migrationBuilder.RenameIndex(
                name: "IX_Quizzes_QuizMediaId",
                table: "Quizzes",
                newName: "IX_Quizzes_MediaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Media_MediaId",
                table: "Quizzes",
                column: "MediaId",
                principalTable: "Media",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Media_MediaId",
                table: "Quizzes");

            migrationBuilder.RenameColumn(
                name: "MediaId",
                table: "Quizzes",
                newName: "QuizMediaId");

            migrationBuilder.RenameIndex(
                name: "IX_Quizzes_MediaId",
                table: "Quizzes",
                newName: "IX_Quizzes_QuizMediaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Media_QuizMediaId",
                table: "Quizzes",
                column: "QuizMediaId",
                principalTable: "Media",
                principalColumn: "Id");
        }
    }
}
