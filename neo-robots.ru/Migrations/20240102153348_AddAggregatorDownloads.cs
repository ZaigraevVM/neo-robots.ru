using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMI.Migrations
{
    /// <inheritdoc />
    public partial class AddAggregatorDownloads : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AggregatorDownloads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AggregatorListId = table.Column<int>(type: "int", nullable: true),
                    AggregatorPageId = table.Column<int>(type: "int", nullable: true),
                    RequestUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseHtml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    History = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AggregatorDownloads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AggregatorDownloads_AggregatorLists_AggregatorListId",
                        column: x => x.AggregatorListId,
                        principalTable: "AggregatorLists",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AggregatorDownloads_AggregatorPages_AggregatorPageId",
                        column: x => x.AggregatorPageId,
                        principalTable: "AggregatorPages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AggregatorDownloads_AggregatorListId",
                table: "AggregatorDownloads",
                column: "AggregatorListId");

            migrationBuilder.CreateIndex(
                name: "IX_AggregatorDownloads_AggregatorPageId",
                table: "AggregatorDownloads",
                column: "AggregatorPageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AggregatorDownloads");
        }
    }
}
