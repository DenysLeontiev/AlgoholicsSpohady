using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserMemoryLikefix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLikes_AspNetUsers_LikedByUsersId",
                table: "UserLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLikes_Memories_LikedMemoriesId",
                table: "UserLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLikes",
                table: "UserLikes");

            migrationBuilder.DropIndex(
                name: "IX_UserLikes_LikedByUsersId",
                table: "UserLikes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserLikes");

            migrationBuilder.DropColumn(
                name: "LikedByUserId",
                table: "UserLikes");

            migrationBuilder.DropColumn(
                name: "LikedMemoryId",
                table: "UserLikes");

            migrationBuilder.RenameColumn(
                name: "LikedMemoriesId",
                table: "UserLikes",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "LikedByUsersId",
                table: "UserLikes",
                newName: "MemoryId");

            migrationBuilder.RenameIndex(
                name: "IX_UserLikes_LikedMemoriesId",
                table: "UserLikes",
                newName: "IX_UserLikes_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLikes",
                table: "UserLikes",
                columns: new[] { "MemoryId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserLikes_AspNetUsers_UserId",
                table: "UserLikes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLikes_Memories_MemoryId",
                table: "UserLikes",
                column: "MemoryId",
                principalTable: "Memories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLikes_AspNetUsers_UserId",
                table: "UserLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLikes_Memories_MemoryId",
                table: "UserLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLikes",
                table: "UserLikes");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserLikes",
                newName: "LikedMemoriesId");

            migrationBuilder.RenameColumn(
                name: "MemoryId",
                table: "UserLikes",
                newName: "LikedByUsersId");

            migrationBuilder.RenameIndex(
                name: "IX_UserLikes_UserId",
                table: "UserLikes",
                newName: "IX_UserLikes_LikedMemoriesId");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "UserLikes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LikedByUserId",
                table: "UserLikes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LikedMemoryId",
                table: "UserLikes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLikes",
                table: "UserLikes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserLikes_LikedByUsersId",
                table: "UserLikes",
                column: "LikedByUsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLikes_AspNetUsers_LikedByUsersId",
                table: "UserLikes",
                column: "LikedByUsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLikes_Memories_LikedMemoriesId",
                table: "UserLikes",
                column: "LikedMemoriesId",
                principalTable: "Memories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
