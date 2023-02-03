using Microsoft.EntityFrameworkCore.Migrations;

namespace App_course.Migrations
{
    public partial class EditProductDelSell : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sale",
                table: "Product");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Sale",
                table: "Product",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
