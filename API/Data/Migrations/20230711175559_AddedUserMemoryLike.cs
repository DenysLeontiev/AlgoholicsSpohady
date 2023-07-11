using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserMemoryLike : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserLikes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    LikedByUserId = table.Column<string>(type: "TEXT", nullable: true),
                    LikedMemoryId = table.Column<string>(type: "TEXT", nullable: true),
                    LikedByUsersId = table.Column<string>(type: "TEXT", nullable: false),
                    LikedMemoriesId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLikes_AspNetUsers_LikedByUsersId",
                        column: x => x.LikedByUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLikes_Memories_LikedMemoriesId",
                        column: x => x.LikedMemoriesId,
                        principalTable: "Memories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserLikes_LikedByUsersId",
                table: "UserLikes",
                column: "LikedByUsersId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLikes_LikedMemoriesId",
                table: "UserLikes",
                column: "LikedMemoriesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLikes");
        }
    }
}
