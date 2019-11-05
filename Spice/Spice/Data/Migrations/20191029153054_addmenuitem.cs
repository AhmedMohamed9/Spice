using Microsoft.EntityFrameworkCore.Migrations;

namespace Spice.Data.Migrations
{
    public partial class addmenuitem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Menuitem",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: false),
                    image = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    spiceness = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    categoryid = table.Column<int>(nullable: false),
                    subcategoryid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menuitem", x => x.id);
                    table.ForeignKey(
                        name: "FK_Menuitem_categories_categoryid",
                        column: x => x.categoryid,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Menuitem_subcategory_subcategoryid",
                        column: x => x.subcategoryid,
                        principalTable: "subcategory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Menuitem_categoryid",
                table: "Menuitem",
                column: "categoryid");

            migrationBuilder.CreateIndex(
                name: "IX_Menuitem_subcategoryid",
                table: "Menuitem",
                column: "subcategoryid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Menuitem");
        }
    }
}
