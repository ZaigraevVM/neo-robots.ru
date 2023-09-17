using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMI.Migrations
{
    public partial class addAggregatorTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AggregatorNews",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AggregatorNews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "History",
                table: "AggregatorNews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "AggregatorNews",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "AggregatorNews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AggregatorLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkHtmlPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    History = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AggregatorSourceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AggregatorLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AggregatorLists_AggregatorSources_AggregatorSourceId",
                        column: x => x.AggregatorSourceId,
                        principalTable: "AggregatorSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AggregatorPages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UrlRegex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleHtmlPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextHtmlPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageHtmlPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateHtmlPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SouceUrlHtmlPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SouceTitleHtmlPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    History = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AggregatorSourceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AggregatorPages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AggregatorPages_AggregatorSources_AggregatorSourceId",
                        column: x => x.AggregatorSourceId,
                        principalTable: "AggregatorSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AggregatorLists_AggregatorSourceId",
                table: "AggregatorLists",
                column: "AggregatorSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_AggregatorPages_AggregatorSourceId",
                table: "AggregatorPages",
                column: "AggregatorSourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AggregatorLists");

            migrationBuilder.DropTable(
                name: "AggregatorPages");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AggregatorNews");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AggregatorNews");

            migrationBuilder.DropColumn(
                name: "History",
                table: "AggregatorNews");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "AggregatorNews");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "AggregatorNews");
        }
    }
}
