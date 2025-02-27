using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AstroWheelAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddRecipeBookIdToPlayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecipeBookId",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Players_RecipeBookId",
                table: "Players",
                column: "RecipeBookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_RecipeBooks_RecipeBookId",
                table: "Players",
                column: "RecipeBookId",
                principalTable: "RecipeBooks",
                principalColumn: "RecipeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_RecipeBooks_RecipeBookId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_RecipeBookId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "RecipeBookId",
                table: "Players");
        }
    }
}
