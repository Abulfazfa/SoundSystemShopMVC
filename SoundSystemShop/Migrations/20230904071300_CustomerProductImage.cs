using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoundSystemShop.Migrations
{
    public partial class CustomerProductImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_CustomerProducts_CustomerProductId",
                table: "ProductImages");

            migrationBuilder.DropIndex(
                name: "IX_ProductImages_CustomerProductId",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "CustomerProductId",
                table: "ProductImages");

            migrationBuilder.CreateTable(
                name: "CustomerProductImage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerProductId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerProductImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerProductImage_CustomerProducts_CustomerProductId",
                        column: x => x.CustomerProductId,
                        principalTable: "CustomerProducts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSeen = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMessages", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "85f67325-b0b2-4bd8-b5dc-972487f5ada5", "7752a0de-ece6-45ea-a4e2-2922f12bc8f1" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProductImage_CustomerProductId",
                table: "CustomerProductImage",
                column: "CustomerProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerProductImage");

            migrationBuilder.DropTable(
                name: "UserMessages");

            migrationBuilder.AddColumn<int>(
                name: "CustomerProductId",
                table: "ProductImages",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "478131f7-d5ad-4439-99e3-fe501b63c7db", "9fdef1bf-52e2-4b63-9898-1456af8ce487" });

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
    }
}
