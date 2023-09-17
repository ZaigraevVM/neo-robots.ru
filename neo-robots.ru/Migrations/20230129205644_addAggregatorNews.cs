using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMI.Migrations
{
    public partial class addAggregatorNews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AggregatorNewsId",
                table: "News",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AggregatorNews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Html = table.Column<string>(type: "text", nullable: true),
                    AggregatorSourceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AggregatorNews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AggregatorNews_AggregatorSourceId",
                        column: x => x.AggregatorSourceId,
                        principalTable: "AggregatorSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_News_AggregatorNewsId",
                table: "News",
                column: "AggregatorNewsId",
                unique: true,
                filter: "[AggregatorNewsId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AggregatorNews_AggregatorSourceId",
                table: "AggregatorNews",
                column: "AggregatorSourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_AggregatorNews_AggregatorNews",
                table: "News",
                column: "AggregatorNewsId",
                principalTable: "AggregatorNews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AggregatorNews_AggregatorNews",
                table: "News");

            migrationBuilder.DropTable(
                name: "AggregatorNews");

            migrationBuilder.DropIndex(
                name: "IX_News_AggregatorNewsId",
                table: "News");

            migrationBuilder.DropColumn(
                name: "AggregatorNewsId",
                table: "News");
        }
    }
}
