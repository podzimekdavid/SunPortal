using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SunPortal.Cloud.Service.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemoveValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "Logs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "Logs",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
