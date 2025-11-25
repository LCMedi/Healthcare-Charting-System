using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.ChartingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddMedicalNotesSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalNote_Patients_PatientId",
                table: "MedicalNote");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalNote_Physician_PhysicianId",
                table: "MedicalNote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MedicalNote",
                table: "MedicalNote");

            migrationBuilder.RenameTable(
                name: "MedicalNote",
                newName: "MedicalNotes");

            migrationBuilder.RenameIndex(
                name: "IX_MedicalNote_PhysicianId",
                table: "MedicalNotes",
                newName: "IX_MedicalNotes_PhysicianId");

            migrationBuilder.RenameIndex(
                name: "IX_MedicalNote_PatientId",
                table: "MedicalNotes",
                newName: "IX_MedicalNotes_PatientId");

            migrationBuilder.AlterColumn<int>(
                name: "PatientId",
                table: "MedicalNotes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedicalNotes",
                table: "MedicalNotes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalNotes_Patients_PatientId",
                table: "MedicalNotes",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalNotes_Physician_PhysicianId",
                table: "MedicalNotes",
                column: "PhysicianId",
                principalTable: "Physician",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalNotes_Patients_PatientId",
                table: "MedicalNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalNotes_Physician_PhysicianId",
                table: "MedicalNotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MedicalNotes",
                table: "MedicalNotes");

            migrationBuilder.RenameTable(
                name: "MedicalNotes",
                newName: "MedicalNote");

            migrationBuilder.RenameIndex(
                name: "IX_MedicalNotes_PhysicianId",
                table: "MedicalNote",
                newName: "IX_MedicalNote_PhysicianId");

            migrationBuilder.RenameIndex(
                name: "IX_MedicalNotes_PatientId",
                table: "MedicalNote",
                newName: "IX_MedicalNote_PatientId");

            migrationBuilder.AlterColumn<int>(
                name: "PatientId",
                table: "MedicalNote",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedicalNote",
                table: "MedicalNote",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalNote_Patients_PatientId",
                table: "MedicalNote",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalNote_Physician_PhysicianId",
                table: "MedicalNote",
                column: "PhysicianId",
                principalTable: "Physician",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
