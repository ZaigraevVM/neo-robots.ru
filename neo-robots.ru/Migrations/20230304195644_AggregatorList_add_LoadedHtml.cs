using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMI.Migrations
{
    public partial class AggregatorList_add_LoadedHtml : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LoadedDate",
                table: "AggregatorLists",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoadedHtml",
                table: "AggregatorLists",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoadedDate",
                table: "AggregatorLists");

            migrationBuilder.DropColumn(
                name: "LoadedHtml",
                table: "AggregatorLists");
        }
    }
}
