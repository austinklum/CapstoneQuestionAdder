using Microsoft.EntityFrameworkCore.Migrations;

namespace ImmersiveQuiz.Migrations.Location
{
    public partial class ImagePath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "Location",
                newName: "ImageExtension");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageExtension",
                table: "Location",
                newName: "FilePath");
        }
    }
}
