using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROJECT_K.Migrations
{
    /// <inheritdoc />
    public partial class BookingHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingHistory_Properties_PropertiesPropertyId",
                table: "BookingHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingHistory_UserInfo_UsersGuid",
                table: "BookingHistory");

            migrationBuilder.DropIndex(
                name: "IX_BookingHistory_PropertiesPropertyId",
                table: "BookingHistory");

            migrationBuilder.DropIndex(
                name: "IX_BookingHistory_UsersGuid",
                table: "BookingHistory");

            migrationBuilder.RenameColumn(
                name: "UsersGuid",
                table: "BookingHistory",
                newName: "Users");

            migrationBuilder.RenameColumn(
                name: "PropertiesPropertyId",
                table: "BookingHistory",
                newName: "Properties");

            migrationBuilder.AlterColumn<double>(
                name: "TotalAmount",
                table: "BookingHistory",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<double>(
                name: "PendingAmount",
                table: "BookingHistory",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Facing",
                table: "BookingHistory",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Users",
                table: "BookingHistory",
                newName: "UsersGuid");

            migrationBuilder.RenameColumn(
                name: "Properties",
                table: "BookingHistory",
                newName: "PropertiesPropertyId");

            migrationBuilder.AlterColumn<string>(
                name: "TotalAmount",
                table: "BookingHistory",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "PendingAmount",
                table: "BookingHistory",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "Facing",
                table: "BookingHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookingHistory_PropertiesPropertyId",
                table: "BookingHistory",
                column: "PropertiesPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingHistory_UsersGuid",
                table: "BookingHistory",
                column: "UsersGuid");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingHistory_Properties_PropertiesPropertyId",
                table: "BookingHistory",
                column: "PropertiesPropertyId",
                principalTable: "Properties",
                principalColumn: "PropertyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingHistory_UserInfo_UsersGuid",
                table: "BookingHistory",
                column: "UsersGuid",
                principalTable: "UserInfo",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
