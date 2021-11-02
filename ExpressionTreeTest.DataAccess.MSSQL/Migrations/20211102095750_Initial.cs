using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpressionTreeTest.DataAccess.MSSQL.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Phones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    ReleaseYear = table.Column<int>(type: "int", nullable: true),
                    SimCardCount = table.Column<int>(type: "int", nullable: true),
                    SimCardFormatId = table.Column<int>(type: "int", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    ScreenDiagonal = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SimCardFormats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Width = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimCardFormats", x => x.Id);
                });

            // добавлено вручную
            migrationBuilder.InsertData(
                table: "SimCardFormats",
                columns: new[] { "Name", "Height", "Width" },
                values: new object[,] {
                    { "Полноразмерные (1FF)", "85.6", "53.98" },
                    { "Mini-SIM (2FF)", "25", "15" },
                    { "Micro-SIM (3FF)", "15", "12" },
                    { "Nano-SIM (4FF)", "12.3", "8.8" },
                    { "Встроенные SIM (Embedded-SIM)", "6", "5" },
                }
            );

            migrationBuilder.InsertData(
                table: "Phones",
                columns: new[] { "Name", "ReleaseYear", "SimCardCount", "SimCardFormatId", "Color", "ScreenDiagonal" },
                values: new object[,] {
                    { "DEXP A440", "2021", "2", "4", "розовый", "4" },
                    { "Samsung Galaxy A72", "2021", "2", "4", "лаванда", "6.7" },
                    { "POCO X3 Pro", "2021", "2", "4", "бежевый", "6.67" }
                }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Phones");

            migrationBuilder.DropTable(
                name: "SimCardFormats");
        }
    }
}
