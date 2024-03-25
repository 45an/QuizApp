using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizApp.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class mediaHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Hash",
                table: "Media",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hash",
                table: "Media");
        }
    }
}
