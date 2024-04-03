using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizApp.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class QuizMedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuizMediaId",
                table: "Quizzes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_QuizMediaId",
                table: "Quizzes",
                column: "QuizMediaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Media_QuizMediaId",
                table: "Quizzes",
                column: "QuizMediaId",
                principalTable: "Media",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Media_QuizMediaId",
                table: "Quizzes");

            migrationBuilder.DropIndex(
                name: "IX_Quizzes_QuizMediaId",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "QuizMediaId",
                table: "Quizzes");
        }
    }
}
