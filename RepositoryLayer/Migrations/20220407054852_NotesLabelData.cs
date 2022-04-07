using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryLayer.Migrations
{
    public partial class NotesLabelData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LabelsData",
                columns: table => new
                {
                    LabelId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LabelNameId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    NoteId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabelsData", x => x.LabelId);
                    table.ForeignKey(
                        name: "FK_LabelsData_LabelsNameData_LabelNameId",
                        column: x => x.LabelNameId,
                        principalTable: "LabelsNameData",
                        principalColumn: "LabelNameId",
                        onDelete: ReferentialAction.NoAction);
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
                name: "IX_LabelsData_LabelNameId",
                table: "LabelsData",
                column: "LabelNameId");

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
