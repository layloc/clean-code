using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetSemester.Migrations.Document
{
    /// <inheritdoc />
    public partial class AddDocumentUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Document",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "Document");
        }
    }
}
