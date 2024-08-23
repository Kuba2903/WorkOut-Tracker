using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class alterExercise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Repetition",
                table: "Exercises",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Sets",
                table: "Exercises",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Weight",
                table: "Exercises",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Repetition",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "Sets",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Exercises");
        }
    }
}
