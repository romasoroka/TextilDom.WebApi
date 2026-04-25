using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Textildom.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveColourJsonColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Colour",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Colour",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
