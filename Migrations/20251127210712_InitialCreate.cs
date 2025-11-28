using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WinFormsApp2.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PivotResults",
                columns: table => new
                {
                    Fsg_GroupId = table.Column<int>(type: "int", nullable: false),
                    Row1 = table.Column<int>(type: "int", nullable: true),
                    Row2 = table.Column<int>(type: "int", nullable: true),
                    Row3 = table.Column<int>(type: "int", nullable: true),
                    Row4 = table.Column<int>(type: "int", nullable: true),
                    Row5 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "TMixedZoneSteelGroup",
                columns: table => new
                {
                    Fsg_GroupId = table.Column<int>(type: "int", nullable: false),
                    Fsg_Row = table.Column<int>(type: "int", nullable: false),
                    Fsg_MixedZoneSteelGroup = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TMixedZoneSteelGroup", x => new { x.Fsg_GroupId, x.Fsg_Row });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PivotResults");

            migrationBuilder.DropTable(
                name: "TMixedZoneSteelGroup");
        }
    }
}
