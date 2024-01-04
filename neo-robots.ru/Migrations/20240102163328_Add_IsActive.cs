using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMI.Migrations
{
    /// <inheritdoc />
    public partial class Add_IsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AggregatorPages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AggregatorLists",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AggregatorPages");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AggregatorLists");
        }
    }
}
