using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAcess.Migrations.ArabicDb
{
    /// <inheritdoc />
    public partial class Arabic_Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "المكونات",
                columns: table => new
                {
                    بطاقةتعريف = table.Column<int>(name: "بطاقة تعريف", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    المكونات = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    النوع = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_المكونات", x => x.بطاقةتعريف);
                });

            migrationBuilder.CreateTable(
                name: "الوصفات",
                columns: table => new
                {
                    بطاقةتعريف = table.Column<int>(name: "بطاقة تعريف", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    اسم = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    وصف = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    طريقةالتحضير = table.Column<string>(name: "طريقة التحضير", type: "nvarchar(max)", nullable: true),
                    دقائق = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_الوصفات", x => x.بطاقةتعريف);
                });

            migrationBuilder.CreateTable(
                name: "وصفة_خام_DTO",
                columns: table => new
                {
                    بطاقة_تعريف = table.Column<int>(type: "int", nullable: false),
                    اسم = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    وصف = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    طريقة_التحضير = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    الوقت = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    النوع = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    سعرات_حرارية_لكل100جم = table.Column<double>(type: "float", nullable: false),
                    دهون_لكل100جم = table.Column<double>(type: "float", nullable: false),
                    سكر_لكل100جم = table.Column<double>(type: "float", nullable: false),
                    بروتين_لكل100جم = table.Column<double>(type: "float", nullable: false),
                    كربوهيدرات_لكل100جم = table.Column<double>(type: "float", nullable: false),
                    اسم_المكون = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "التغذية",
                columns: table => new
                {
                    بطاقةتعريف = table.Column<int>(name: "بطاقة تعريف", type: "int", nullable: false),
                    النوع = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    السعراتالحرارية = table.Column<double>(name: "السعرات الحرارية", type: "float", nullable: false),
                    سمين = table.Column<double>(type: "float", nullable: false),
                    سكر = table.Column<double>(type: "float", nullable: false),
                    بروتين = table.Column<double>(type: "float", nullable: false),
                    الكربوهيدرات = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_التغذية", x => x.بطاقةتعريف);
                    table.ForeignKey(
                        name: "FK_التغذية_الوصفات_بطاقة تعريف",
                        column: x => x.بطاقةتعريف,
                        principalTable: "الوصفات",
                        principalColumn: "بطاقة تعريف",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "وصفة_المكونات",
                columns: table => new
                {
                    بطاقةتعريفالوصفة = table.Column<int>(name: "بطاقة تعريف الوصفة", type: "int", nullable: false),
                    بطاقةتعريفالمكون = table.Column<int>(name: "بطاقة تعريف المكون", type: "int", nullable: false),
                    كمية = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_وصفة_المكونات", x => new { x.بطاقةتعريفالوصفة, x.بطاقةتعريفالمكون });
                    table.ForeignKey(
                        name: "FK_وصفة_المكونات_المكونات_بطاقة تعريف المكون",
                        column: x => x.بطاقةتعريفالمكون,
                        principalTable: "المكونات",
                        principalColumn: "بطاقة تعريف",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_وصفة_المكونات_الوصفات_بطاقة تعريف الوصفة",
                        column: x => x.بطاقةتعريفالوصفة,
                        principalTable: "الوصفات",
                        principalColumn: "بطاقة تعريف",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_وصفة_المكونات_بطاقة تعريف المكون",
                table: "وصفة_المكونات",
                column: "بطاقة تعريف المكون");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "التغذية");

            migrationBuilder.DropTable(
                name: "وصفة_المكونات");

            migrationBuilder.DropTable(
                name: "وصفة_خام_DTO");

            migrationBuilder.DropTable(
                name: "المكونات");

            migrationBuilder.DropTable(
                name: "الوصفات");
        }
    }
}
