using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizApp.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class mediaGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Guid",
                table: "Media",
                newName: "MediaGuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MediaGuid",
                table: "Media",
                newName: "Guid");
        }
    }
}
