using Microsoft.EntityFrameworkCore.Migrations;

namespace SMI.Migrations
{
    public partial class ChangeThemes_AddSorting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sorting",
                table: "Themes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sorting",
                table: "Themes");
        }
    }
}
