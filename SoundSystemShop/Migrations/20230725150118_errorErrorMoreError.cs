using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoundSystemShop.Migrations
{
    public partial class errorErrorMoreError : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "BlogComments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Products_AppUserId",
                table: "Products",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_AppUserId",
                table: "Products",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_AppUserId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_AppUserId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "BlogComments");
        }
    }
}
