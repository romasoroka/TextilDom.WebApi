using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Textildom.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Supabas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completeness",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ItemCount",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "PaymentType",
                table: "Orders",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Online");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "Completeness",
                table: "Products",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemCount",
                table: "Products",
                type: "integer",
                nullable: true);
        }
    }
}
