using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryLayer.Migrations
{
    public partial class CollabsUsersNotesData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CollabId",
                table: "CollabUserNotesData",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_CollabUserNotesData_CollabId",
                table: "CollabUserNotesData",
                column: "CollabId");

            migrationBuilder.AddForeignKey(
                name: "FK_CollabUserNotesData_CollaboratorData_CollabId",
                table: "CollabUserNotesData",
                column: "CollabId",
                principalTable: "CollaboratorData",
                principalColumn: "CollabId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CollabUserNotesData_CollaboratorData_CollabId",
                table: "CollabUserNotesData");

            migrationBuilder.DropIndex(
                name: "IX_CollabUserNotesData_CollabId",
                table: "CollabUserNotesData");

            migrationBuilder.DropColumn(
                name: "CollabId",
                table: "CollabUserNotesData");
        }
    }
}
