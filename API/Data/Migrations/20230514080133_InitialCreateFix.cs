using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMemory_AspNetUsers_UsersId",
                table: "UserMemory");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMemory_Memory_MemoriesId",
                table: "UserMemory");

            migrationBuilder.DropIndex(
                name: "IX_UserMemory_MemoriesId",
                table: "UserMemory");

            migrationBuilder.DropIndex(
                name: "IX_UserMemory_UsersId",
                table: "UserMemory");

            migrationBuilder.DropColumn(
                name: "MemoriesId",
                table: "UserMemory");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "UserMemory");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MemoriesId",
                table: "UserMemory",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UsersId",
                table: "UserMemory",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UserMemory_MemoriesId",
                table: "UserMemory",
                column: "MemoriesId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMemory_UsersId",
                table: "UserMemory",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMemory_AspNetUsers_UsersId",
                table: "UserMemory",
                column: "UsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMemory_Memory_MemoriesId",
                table: "UserMemory",
                column: "MemoriesId",
                principalTable: "Memory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
