using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SunPortal.Cloud.Service.Database.Migrations
{
    /// <inheritdoc />
    public partial class Charts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Charts_Parameters_YParameterId",
                table: "Charts");

            migrationBuilder.RenameColumn(
                name: "YParameterId",
                table: "Charts",
                newName: "PrimaryYParameterId");

            migrationBuilder.RenameIndex(
                name: "IX_Charts_YParameterId",
                table: "Charts",
                newName: "IX_Charts_PrimaryYParameterId");

            migrationBuilder.AddColumn<int>(
                name: "ChartType",
                table: "Charts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Charts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SecondaryYParameterId",
                table: "Charts",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Sharing",
                columns: table => new
                {
                    ClientSharingId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sharing", x => x.ClientSharingId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Charts_SecondaryYParameterId",
                table: "Charts",
                column: "SecondaryYParameterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Charts_Parameters_PrimaryYParameterId",
                table: "Charts",
                column: "PrimaryYParameterId",
                principalTable: "Parameters",
                principalColumn: "ParameterId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Charts_Parameters_SecondaryYParameterId",
                table: "Charts",
                column: "SecondaryYParameterId",
                principalTable: "Parameters",
                principalColumn: "ParameterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Charts_Parameters_PrimaryYParameterId",
                table: "Charts");

            migrationBuilder.DropForeignKey(
                name: "FK_Charts_Parameters_SecondaryYParameterId",
                table: "Charts");

            migrationBuilder.DropTable(
                name: "Sharing");

            migrationBuilder.DropIndex(
                name: "IX_Charts_SecondaryYParameterId",
                table: "Charts");

            migrationBuilder.DropColumn(
                name: "ChartType",
                table: "Charts");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Charts");

            migrationBuilder.DropColumn(
                name: "SecondaryYParameterId",
                table: "Charts");

            migrationBuilder.RenameColumn(
                name: "PrimaryYParameterId",
                table: "Charts",
                newName: "YParameterId");

            migrationBuilder.RenameIndex(
                name: "IX_Charts_PrimaryYParameterId",
                table: "Charts",
                newName: "IX_Charts_YParameterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Charts_Parameters_YParameterId",
                table: "Charts",
                column: "YParameterId",
                principalTable: "Parameters",
                principalColumn: "ParameterId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
