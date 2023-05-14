using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class NewMemoryField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photo_Memory_MemoryId",
                table: "Photo");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMemory_AspNetUsers_UserId",
                table: "UserMemory");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMemory_Memory_MemoryId",
                table: "UserMemory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMemory",
                table: "UserMemory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Photo",
                table: "Photo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Memory",
                table: "Memory");

            migrationBuilder.RenameTable(
                name: "UserMemory",
                newName: "UserMemories");

            migrationBuilder.RenameTable(
                name: "Photo",
                newName: "Photos");

            migrationBuilder.RenameTable(
                name: "Memory",
                newName: "Memories");

            migrationBuilder.RenameIndex(
                name: "IX_UserMemory_UserId",
                table: "UserMemories",
                newName: "IX_UserMemories_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserMemory_MemoryId",
                table: "UserMemories",
                newName: "IX_UserMemories_MemoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Photo_MemoryId",
                table: "Photos",
                newName: "IX_Photos_MemoryId");

            migrationBuilder.AddColumn<string>(
                name: "OwnerUserName",
                table: "Memories",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMemories",
                table: "UserMemories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Photos",
                table: "Photos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Memories",
                table: "Memories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Memories_MemoryId",
                table: "Photos",
                column: "MemoryId",
                principalTable: "Memories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMemories_AspNetUsers_UserId",
                table: "UserMemories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMemories_Memories_MemoryId",
                table: "UserMemories",
                column: "MemoryId",
                principalTable: "Memories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Memories_MemoryId",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMemories_AspNetUsers_UserId",
                table: "UserMemories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMemories_Memories_MemoryId",
                table: "UserMemories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMemories",
                table: "UserMemories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Photos",
                table: "Photos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Memories",
                table: "Memories");

            migrationBuilder.DropColumn(
                name: "OwnerUserName",
                table: "Memories");

            migrationBuilder.RenameTable(
                name: "UserMemories",
                newName: "UserMemory");

            migrationBuilder.RenameTable(
                name: "Photos",
                newName: "Photo");

            migrationBuilder.RenameTable(
                name: "Memories",
                newName: "Memory");

            migrationBuilder.RenameIndex(
                name: "IX_UserMemories_UserId",
                table: "UserMemory",
                newName: "IX_UserMemory_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserMemories_MemoryId",
                table: "UserMemory",
                newName: "IX_UserMemory_MemoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Photos_MemoryId",
                table: "Photo",
                newName: "IX_Photo_MemoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMemory",
                table: "UserMemory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Photo",
                table: "Photo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Memory",
                table: "Memory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Photo_Memory_MemoryId",
                table: "Photo",
                column: "MemoryId",
                principalTable: "Memory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMemory_AspNetUsers_UserId",
                table: "UserMemory",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMemory_Memory_MemoryId",
                table: "UserMemory",
                column: "MemoryId",
                principalTable: "Memory",
                principalColumn: "Id");
        }
    }
}
