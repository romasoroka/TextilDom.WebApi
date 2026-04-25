using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Textildom.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductToUseVariants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Size",
                table: "Products",
                newName: "Variants");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Variants",
                table: "Products",
                newName: "Size");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
