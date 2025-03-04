using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace AstroWheelAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePlayerDTO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Material01",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "Material02",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "Material03",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "Material04",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "Material05",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "Material06",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "Material07",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "Material08",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "Material09",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "Material10",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "Material11",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "Material12",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "Material13",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "Material14",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "Material15",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "Material16",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "Material17",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "Material18",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "Material19",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "Material20",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "Material21",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "Material22",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "Material23",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "Material24",
                table: "Inventories");

            migrationBuilder.AlterColumn<int>(
                name: "MaterialId",
                table: "Materials",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.CreateTable(
                name: "InventoryMaterials",
                columns: table => new
                {
                    InventoryId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryMaterials", x => new { x.InventoryId, x.MaterialId });
                    table.ForeignKey(
                        name: "FK_InventoryMaterials_Inventories_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "Inventories",
                        principalColumn: "InventoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryMaterials_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "MaterialId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryMaterials_MaterialId",
                table: "InventoryMaterials",
                column: "MaterialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryMaterials");

            migrationBuilder.AlterColumn<string>(
                name: "MaterialId",
                table: "Materials",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "Material01",
                table: "Inventories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Material02",
                table: "Inventories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Material03",
                table: "Inventories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Material04",
                table: "Inventories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Material05",
                table: "Inventories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Material06",
                table: "Inventories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Material07",
                table: "Inventories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Material08",
                table: "Inventories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Material09",
                table: "Inventories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Material10",
                table: "Inventories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Material11",
                table: "Inventories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Material12",
                table: "Inventories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Material13",
                table: "Inventories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Material14",
                table: "Inventories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Material15",
                table: "Inventories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Material16",
                table: "Inventories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Material17",
                table: "Inventories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Material18",
                table: "Inventories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Material19",
                table: "Inventories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Material20",
                table: "Inventories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Material21",
                table: "Inventories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Material22",
                table: "Inventories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Material23",
                table: "Inventories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Material24",
                table: "Inventories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
