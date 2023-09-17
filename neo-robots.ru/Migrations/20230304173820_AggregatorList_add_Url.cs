using Microsoft.EntityFrameworkCore.Migrations;

namespace SMI.Migrations
{
    public partial class AggregatorList_add_Url : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "AggregatorLists",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "AggregatorLists");
        }
    }
}
