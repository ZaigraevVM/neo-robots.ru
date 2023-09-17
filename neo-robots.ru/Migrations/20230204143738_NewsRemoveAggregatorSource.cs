using Microsoft.EntityFrameworkCore.Migrations;

namespace SMI.Migrations
{
    public partial class NewsRemoveAggregatorSource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_AggregatorSources",
                table: "News");

            migrationBuilder.DropIndex(
                name: "IX_News_AggregatorSourceId",
                table: "News");

            migrationBuilder.DropColumn(
                name: "AggregatorSourceId",
                table: "News");

            migrationBuilder.RenameColumn(
                name: "RssUrl",
                table: "AggregatorSources",
                newName: "Url");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "AggregatorSources",
                newName: "RssUrl");

            migrationBuilder.AddColumn<int>(
                name: "AggregatorSourceId",
                table: "News",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_News_AggregatorSourceId",
                table: "News",
                column: "AggregatorSourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_News_AggregatorSources",
                table: "News",
                column: "AggregatorSourceId",
                principalTable: "AggregatorSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
