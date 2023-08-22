using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoundSystemShop.Migrations
{
    public partial class ProductQuality : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Quality",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "753a3c65-4e35-4f4b-8249-16e20bc5795a", "ef47303e-0df1-418c-840c-b6487aa28350" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quality",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "63a7d22f-2ffb-474d-88a6-d744d45f8870", "d2c808c5-e892-4ab8-8b1b-addd094d851c" });
        }
    }
}
