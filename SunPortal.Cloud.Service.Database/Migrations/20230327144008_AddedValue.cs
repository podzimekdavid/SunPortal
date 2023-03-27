using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SunPortal.Cloud.Service.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddedValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Value",
                table: "Logs",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "Logs");
        }
    }
}
