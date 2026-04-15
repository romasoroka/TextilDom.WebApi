using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Luzanov.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCrea2e : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemNumber",
                table: "Products");

            migrationBuilder.AddColumn<bool>(
                name: "IsSpecialOffer",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTop",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSpecialOffer",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsTop",
                table: "Products");

            migrationBuilder.AddColumn<decimal>(
                name: "ItemNumber",
                table: "Products",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
