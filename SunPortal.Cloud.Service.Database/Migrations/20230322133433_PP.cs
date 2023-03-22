using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SunPortal.Cloud.Service.Database.Migrations
{
    /// <inheritdoc />
    public partial class PP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "Parameters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Charts",
                columns: table => new
                {
                    GroupChartId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    YType = table.Column<int>(type: "integer", nullable: false),
                    YParameterId = table.Column<int>(type: "integer", nullable: false),
                    ParameterGroupId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Charts", x => x.GroupChartId);
                    table.ForeignKey(
                        name: "FK_Charts_ParameterGroups_ParameterGroupId",
                        column: x => x.ParameterGroupId,
                        principalTable: "ParameterGroups",
                        principalColumn: "ParameterGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Charts_Parameters_YParameterId",
                        column: x => x.YParameterId,
                        principalTable: "Parameters",
                        principalColumn: "ParameterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Charts_ParameterGroupId",
                table: "Charts",
                column: "ParameterGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Charts_YParameterId",
                table: "Charts",
                column: "YParameterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Charts");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Parameters");
        }
    }
}
