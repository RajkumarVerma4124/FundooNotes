using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryLayer.Migrations
{
    public partial class LabelData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LabelsData",
                columns: table => new
                {
                    LabelId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LabelName = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    NoteId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabelsData", x => x.LabelId);
                    table.ForeignKey(
                        name: "FK_LabelsData_NotesData_NoteId",
                        column: x => x.NoteId,
                        principalTable: "NotesData",
                        principalColumn: "NoteId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LabelsData_UserData_UserId",
                        column: x => x.UserId,
                        principalTable: "UserData",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LabelsData_NoteId",
                table: "LabelsData",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_LabelsData_UserId",
                table: "LabelsData",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LabelsData");
        }
    }
}
