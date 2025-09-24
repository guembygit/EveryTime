using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlanningTime.Migrations
{
    /// <inheritdoc />
    public partial class ImageUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUser",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUser",
                table: "Users");
        }
    }
}
