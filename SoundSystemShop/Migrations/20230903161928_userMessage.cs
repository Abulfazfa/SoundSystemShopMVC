using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoundSystemShop.Migrations
{
    public partial class userMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "CustomerProducts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "478131f7-d5ad-4439-99e3-fe501b63c7db", "9fdef1bf-52e2-4b63-9898-1456af8ce487" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProducts_AppUserId",
                table: "CustomerProducts",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerProducts_AspNetUsers_AppUserId",
                table: "CustomerProducts",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerProducts_AspNetUsers_AppUserId",
                table: "CustomerProducts");

            migrationBuilder.DropIndex(
                name: "IX_CustomerProducts_AppUserId",
                table: "CustomerProducts");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "CustomerProducts");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "1358bfaa-d6ae-4e11-bddf-3c0026743561", "85c7f234-a756-4cc7-8b80-b999d79f2d4d" });
        }
    }
}
