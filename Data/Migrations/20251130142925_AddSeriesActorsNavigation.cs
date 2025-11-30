using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TVSeriesApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSeriesActorsNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SeriesId1",
                table: "SeriesActors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "Actors",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesActors_SeriesId1",
                table: "SeriesActors",
                column: "SeriesId1");

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesActors_Series_SeriesId1",
                table: "SeriesActors",
                column: "SeriesId1",
                principalTable: "Series",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeriesActors_Series_SeriesId1",
                table: "SeriesActors");

            migrationBuilder.DropIndex(
                name: "IX_SeriesActors_SeriesId1",
                table: "SeriesActors");

            migrationBuilder.DropColumn(
                name: "SeriesId1",
                table: "SeriesActors");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "Actors");
        }
    }
}
