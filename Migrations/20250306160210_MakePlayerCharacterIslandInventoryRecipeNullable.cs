using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AstroWheelAPI.Migrations
{
    /// <inheritdoc />
    public partial class MakePlayerCharacterIslandInventoryRecipeNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Characters_CharacterId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Inventories_InventoryId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Islands_IslandId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_RecipeBooks_RecipeBookId",
                table: "Players");

            migrationBuilder.AlterColumn<int>(
                name: "RecipeBookId",
                table: "Players",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IslandId",
                table: "Players",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "InventoryId",
                table: "Players",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CharacterId",
                table: "Players",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Characters_CharacterId",
                table: "Players",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "CharacterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Inventories_InventoryId",
                table: "Players",
                column: "InventoryId",
                principalTable: "Inventories",
                principalColumn: "InventoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Islands_IslandId",
                table: "Players",
                column: "IslandId",
                principalTable: "Islands",
                principalColumn: "IslandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_RecipeBooks_RecipeBookId",
                table: "Players",
                column: "RecipeBookId",
                principalTable: "RecipeBooks",
                principalColumn: "RecipeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Characters_CharacterId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Inventories_InventoryId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Islands_IslandId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_RecipeBooks_RecipeBookId",
                table: "Players");

            migrationBuilder.AlterColumn<int>(
                name: "RecipeBookId",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IslandId",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "InventoryId",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CharacterId",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Characters_CharacterId",
                table: "Players",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "CharacterId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Inventories_InventoryId",
                table: "Players",
                column: "InventoryId",
                principalTable: "Inventories",
                principalColumn: "InventoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Islands_IslandId",
                table: "Players",
                column: "IslandId",
                principalTable: "Islands",
                principalColumn: "IslandId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_RecipeBooks_RecipeBookId",
                table: "Players",
                column: "RecipeBookId",
                principalTable: "RecipeBooks",
                principalColumn: "RecipeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
