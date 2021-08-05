using Microsoft.EntityFrameworkCore.Migrations;

namespace Screen_Capture.MVVM.Model
{
    public partial class MyFirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HotKeys",
                columns: table => new
                {
                    HotKeyID = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Ctrl = table.Column<long>(nullable: false),
                    Shift = table.Column<long>(nullable: false),
                    Alt = table.Column<long>(nullable: false),
                    Button = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotKeys", x => x.HotKeyID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HotKeys_HotKeyID",
                table: "HotKeys",
                column: "HotKeyID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HotKeys");
        }
    }
}
