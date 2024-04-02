using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspnetIdentityRoleBasedTutorial.Migrations
{
    /// <inheritdoc />
    public partial class InitRa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StudentId",
                table: "Topics",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Topics_StudentId",
                table: "Topics",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_AspNetUsers_StudentId",
                table: "Topics",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Topics_AspNetUsers_StudentId",
                table: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_Topics_StudentId",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Topics");
        }
    }
}
