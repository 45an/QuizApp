using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizApp.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class Answer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Quizzes_QuizId",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "QuizId",
                table: "Games",
                newName: "AnswerId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_QuizId",
                table: "Games",
                newName: "IX_Games_AnswerId");

            migrationBuilder.AddColumn<bool>(
                name: "CorrectAnswer",
                table: "Questions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Answer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OriginalQuizId = table.Column<int>(type: "int", nullable: true),
                    AnswerQuizId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answer_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Answer_Quizzes_AnswerQuizId",
                        column: x => x.AnswerQuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Answer_Quizzes_OriginalQuizId",
                        column: x => x.OriginalQuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answer_AnswerQuizId",
                table: "Answer",
                column: "AnswerQuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_OriginalQuizId",
                table: "Answer",
                column: "OriginalQuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_UserId",
                table: "Answer",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Answer_AnswerId",
                table: "Games",
                column: "AnswerId",
                principalTable: "Answer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Answer_AnswerId",
                table: "Games");

            migrationBuilder.DropTable(
                name: "Answer");

            migrationBuilder.DropColumn(
                name: "CorrectAnswer",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "AnswerId",
                table: "Games",
                newName: "QuizId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_AnswerId",
                table: "Games",
                newName: "IX_Games_QuizId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Quizzes_QuizId",
                table: "Games",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
