using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AstroWheelAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexToCharacterIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Characters_CharacterIndex",
                table: "Characters",
                column: "CharacterIndex",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Characters_CharacterIndex",
                table: "Characters");
        }
    }
}
