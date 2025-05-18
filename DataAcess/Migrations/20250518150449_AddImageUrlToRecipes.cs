using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAcess.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlToRecipes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "التغذية");

            migrationBuilder.DropTable(
                name: "وصفة_المكونات");

            migrationBuilder.DropTable(
                name: "المكونات");

            migrationBuilder.DropTable(
                name: "الوصفات");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Recipe",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Recipe");

            migrationBuilder.CreateTable(
                name: "المكونات",
                columns: table => new
                {
                    بطاقة_المكون = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    اسم_المكون = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_المكونات", x => x.بطاقة_المكون);
                });

            migrationBuilder.CreateTable(
                name: "الوصفات",
                columns: table => new
                {
                    بطاقة_تعريف = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    اسم = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    الوقت = table.Column<int>(type: "int", nullable: false),
                    طريقة_التحضير = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    وصف = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_الوصفات", x => x.بطاقة_تعريف);
                });

            migrationBuilder.CreateTable(
                name: "التغذية",
                columns: table => new
                {
                    بطاقة_الوصفة = table.Column<int>(type: "int", nullable: false),
                    النوع = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    بروتين_لكل100جم = table.Column<double>(type: "float", nullable: false),
                    دهون_لكل100جم = table.Column<double>(type: "float", nullable: false),
                    سعرات_حرارية_لكل100جم = table.Column<double>(type: "float", nullable: false),
                    سكر_لكل100جم = table.Column<double>(type: "float", nullable: false),
                    كربوهيدرات_لكل100جم = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_التغذية", x => x.بطاقة_الوصفة);
                    table.ForeignKey(
                        name: "FK_التغذية_الوصفات_بطاقة_الوصفة",
                        column: x => x.بطاقة_الوصفة,
                        principalTable: "الوصفات",
                        principalColumn: "بطاقة_تعريف",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "وصفة_المكونات",
                columns: table => new
                {
                    بطاقة_الوصفة = table.Column<int>(type: "int", nullable: false),
                    بطاقة_المكون = table.Column<int>(type: "int", nullable: false),
                    كمية = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_وصفة_المكونات", x => new { x.بطاقة_الوصفة, x.بطاقة_المكون });
                    table.ForeignKey(
                        name: "FK_وصفة_المكونات_المكونات_بطاقة_المكون",
                        column: x => x.بطاقة_المكون,
                        principalTable: "المكونات",
                        principalColumn: "بطاقة_المكون",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_وصفة_المكونات_الوصفات_بطاقة_الوصفة",
                        column: x => x.بطاقة_الوصفة,
                        principalTable: "الوصفات",
                        principalColumn: "بطاقة_تعريف",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_وصفة_المكونات_بطاقة_المكون",
                table: "وصفة_المكونات",
                column: "بطاقة_المكون");
        }
    }
}
