using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROJECT_K.Migrations
{
    /// <inheritdoc />
    public partial class adding_column_in_BookingHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Facing",
                table: "BookingHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Facing",
                table: "BookingHistory");
        }
    }
}
