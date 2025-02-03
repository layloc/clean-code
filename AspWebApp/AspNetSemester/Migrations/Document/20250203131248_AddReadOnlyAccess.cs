using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetSemester.Migrations.Document
{
    /// <inheritdoc />
    public partial class AddReadOnlyAccess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReadOnly",
                table: "DocumentAccess",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReadOnly",
                table: "DocumentAccess");
        }
    }
}
