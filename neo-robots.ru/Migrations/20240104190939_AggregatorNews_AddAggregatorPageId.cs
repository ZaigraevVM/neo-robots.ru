using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMI.Migrations
{
    /// <inheritdoc />
    public partial class AggregatorNews_AddAggregatorPageId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AggregatorPageId",
                table: "AggregatorNews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AggregatorNews_AggregatorPageId",
                table: "AggregatorNews",
                column: "AggregatorPageId");

            migrationBuilder.AddForeignKey(
                name: "FK_AggregatorNews_AggregatorPageId",
                table: "AggregatorNews",
                column: "AggregatorPageId",
                principalTable: "AggregatorPages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AggregatorNews_AggregatorPageId",
                table: "AggregatorNews");

            migrationBuilder.DropIndex(
                name: "IX_AggregatorNews_AggregatorPageId",
                table: "AggregatorNews");

            migrationBuilder.DropColumn(
                name: "AggregatorPageId",
                table: "AggregatorNews");
        }
    }
}
