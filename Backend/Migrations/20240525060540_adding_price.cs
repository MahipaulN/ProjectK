using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROJECT_K.Migrations
{
    /// <inheritdoc />
    public partial class adding_price : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "EFPrice",
                table: "Properties",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "WFPrice",
                table: "Properties",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "PendingAmount",
                table: "BookingHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TotalAmount",
                table: "BookingHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EFPrice",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "WFPrice",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "PendingAmount",
                table: "BookingHistory");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "BookingHistory");
        }
    }
}
