using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMI.Migrations
{
    /// <inheritdoc />
    public partial class AggregatorNews_AddAggregatorDownloadId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AggregatorNews_AggregatorPageId",
                table: "AggregatorNews");

            migrationBuilder.RenameColumn(
                name: "AggregatorPageId",
                table: "AggregatorNews",
                newName: "AggregatorDownloadId");

            migrationBuilder.RenameIndex(
                name: "IX_AggregatorNews_AggregatorPageId",
                table: "AggregatorNews",
                newName: "IX_AggregatorNews_AggregatorDownloadId");

            migrationBuilder.AddForeignKey(
                name: "FK_AggregatorNews_AggregatorDownloadId",
                table: "AggregatorNews",
                column: "AggregatorDownloadId",
                principalTable: "AggregatorDownloads",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AggregatorNews_AggregatorDownloadId",
                table: "AggregatorNews");

            migrationBuilder.RenameColumn(
                name: "AggregatorDownloadId",
                table: "AggregatorNews",
                newName: "AggregatorPageId");

            migrationBuilder.RenameIndex(
                name: "IX_AggregatorNews_AggregatorDownloadId",
                table: "AggregatorNews",
                newName: "IX_AggregatorNews_AggregatorPageId");

            migrationBuilder.AddForeignKey(
                name: "FK_AggregatorNews_AggregatorPageId",
                table: "AggregatorNews",
                column: "AggregatorPageId",
                principalTable: "AggregatorPages",
                principalColumn: "Id");
        }
    }
}
