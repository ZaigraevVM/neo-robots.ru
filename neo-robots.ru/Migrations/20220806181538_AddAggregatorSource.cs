using Microsoft.EntityFrameworkCore.Migrations;

namespace SMI.Migrations
{
    public partial class AddAggregatorSource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AggregatorSourceId",
                table: "News",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AggregatorSources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RssUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AggregatorSources", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_AggregatorSources",
                table: "News");

            migrationBuilder.DropTable(
                name: "AggregatorSources");

            migrationBuilder.DropIndex(
                name: "IX_News_AggregatorSourceId",
                table: "News");

            migrationBuilder.DropColumn(
                name: "AggregatorSourceId",
                table: "News");
        }
    }
}
