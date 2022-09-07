using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaApi2.Migrations
{
    public partial class AddingMoviePhotoAndMovieVideoPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MovieThumbNail",
                table: "MoviesTable",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MovieVideoLink",
                table: "MoviesTable",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MovieThumbNail",
                table: "MoviesTable");

            migrationBuilder.DropColumn(
                name: "MovieVideoLink",
                table: "MoviesTable");
        }
    }
}
