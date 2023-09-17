using Microsoft.EntityFrameworkCore.Migrations;

namespace SMI.Migrations
{
    public partial class ChangeNews_AddIntro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Intro",
                table: "News",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Intro",
                table: "News");
        }
    }
}
