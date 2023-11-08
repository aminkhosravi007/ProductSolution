using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Product.API.Migrations
{
    /// <inheritdoc />
    public partial class AdminTokenAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IssuedAdminToken",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IssuedAdminToken",
                table: "Products");
        }
    }
}
