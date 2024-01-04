using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMI.Migrations
{
    /// <inheritdoc />
    public partial class AggregatorDownload_AddError : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Error",
                table: "AggregatorDownloads",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsError",
                table: "AggregatorDownloads",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Error",
                table: "AggregatorDownloads");

            migrationBuilder.DropColumn(
                name: "IsError",
                table: "AggregatorDownloads");
        }
    }
}
