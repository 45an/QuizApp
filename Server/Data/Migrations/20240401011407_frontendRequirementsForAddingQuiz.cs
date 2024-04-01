using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizApp.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class frontendRequirementsForAddingQuiz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "Media",
                table: "Questions");

            migrationBuilder.AddColumn<int>(
                name: "MediaId",
                table: "Questions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_MediaId",
                table: "Questions",
                column: "MediaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Media_MediaId",
                table: "Questions",
                column: "MediaId",
                principalTable: "Media",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Media_MediaId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_MediaId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "MediaId",
                table: "Questions");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Quizzes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Media",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
