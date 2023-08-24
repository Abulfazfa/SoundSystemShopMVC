using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoundSystemShop.Migrations
{
    public partial class CustomerProductNotWorking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerProductId",
                table: "ProductImages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomerProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerProducts", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "42562174-cce4-44d4-81de-d1bd81fe414c", "ad77ba70-d6ce-4278-9f6d-d70aede2c177" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_CustomerProductId",
                table: "ProductImages",
                column: "CustomerProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_CustomerProducts_CustomerProductId",
                table: "ProductImages",
                column: "CustomerProductId",
                principalTable: "CustomerProducts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_CustomerProducts_CustomerProductId",
                table: "ProductImages");

            migrationBuilder.DropTable(
                name: "CustomerProducts");

            migrationBuilder.DropIndex(
                name: "IX_ProductImages_CustomerProductId",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "CustomerProductId",
                table: "ProductImages");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "502a42aa-1b44-4dfd-b2b1-df4befe57cc6", "cb149c08-b04e-4e23-aa9e-54cdd61727b9" });
        }
    }
}
