using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AstroWheelAPI.Migrations
{
    /// <inheritdoc />
    public partial class MakeCharacterRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Characters_CharacterId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_CharacterId",
                table: "Players");

            migrationBuilder.AlterColumn<int>(
                name: "CharacterId",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_CharacterId",
                table: "Players",
                column: "CharacterId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Characters_CharacterId",
                table: "Players",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "CharacterId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Characters_CharacterId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_CharacterId",
                table: "Players");

            migrationBuilder.AlterColumn<int>(
                name: "CharacterId",
                table: "Players",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Players_CharacterId",
                table: "Players",
                column: "CharacterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Characters_CharacterId",
                table: "Players",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "CharacterId");
        }
    }
}
