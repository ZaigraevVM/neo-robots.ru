using Microsoft.EntityFrameworkCore.Migrations;

namespace SMI.Migrations
{
    public partial class ChangePhoto_AddSourceUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HashTagsNews_HashTags",
                table: "HashTagsNews");

            migrationBuilder.DropForeignKey(
                name: "FK_HashTagsNews_News",
                table: "HashTagsNews");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsCities_Cities",
                table: "NewsCities");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsCities_News",
                table: "NewsCities");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsRegions_News",
                table: "NewsRegions");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsRegions_Regions",
                table: "NewsRegions");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsThemes_News",
                table: "NewsThemes");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsThemes_Themes",
                table: "NewsThemes");

            migrationBuilder.AddColumn<string>(
                name: "SourceUrl",
                table: "Photos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HashTagsNews_HashTags",
                table: "HashTagsNews",
                column: "HashTagId",
                principalTable: "HashTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HashTagsNews_News",
                table: "HashTagsNews",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsCities_Cities",
                table: "NewsCities",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsCities_News",
                table: "NewsCities",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsRegions_News",
                table: "NewsRegions",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsRegions_Regions",
                table: "NewsRegions",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsThemes_News",
                table: "NewsThemes",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsThemes_Themes",
                table: "NewsThemes",
                column: "ThemeId",
                principalTable: "Themes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HashTagsNews_HashTags",
                table: "HashTagsNews");

            migrationBuilder.DropForeignKey(
                name: "FK_HashTagsNews_News",
                table: "HashTagsNews");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsCities_Cities",
                table: "NewsCities");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsCities_News",
                table: "NewsCities");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsRegions_News",
                table: "NewsRegions");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsRegions_Regions",
                table: "NewsRegions");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsThemes_News",
                table: "NewsThemes");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsThemes_Themes",
                table: "NewsThemes");

            migrationBuilder.DropColumn(
                name: "SourceUrl",
                table: "Photos");

            migrationBuilder.AddForeignKey(
                name: "FK_HashTagsNews_HashTags",
                table: "HashTagsNews",
                column: "HashTagId",
                principalTable: "HashTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HashTagsNews_News",
                table: "HashTagsNews",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsCities_Cities",
                table: "NewsCities",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsCities_News",
                table: "NewsCities",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsRegions_News",
                table: "NewsRegions",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsRegions_Regions",
                table: "NewsRegions",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsThemes_News",
                table: "NewsThemes",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsThemes_Themes",
                table: "NewsThemes",
                column: "ThemeId",
                principalTable: "Themes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
