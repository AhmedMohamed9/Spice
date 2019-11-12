using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Spice.Data.Migrations
{
    public partial class addorderHeaderandorderdetailstables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderHeaders",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userid = table.Column<string>(nullable: false),
                    orderdate = table.Column<DateTime>(nullable: false),
                    OrdartotalOriginal = table.Column<double>(nullable: false),
                    ordertotal = table.Column<double>(nullable: false),
                    Picktime = table.Column<DateTime>(nullable: false),
                    CouponCode = table.Column<string>(nullable: true),
                    CouponCodeDiscount = table.Column<double>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    PaymentStatus = table.Column<string>(nullable: true),
                    Comments = table.Column<string>(nullable: true),
                    Picupname = table.Column<string>(nullable: true),
                    Phonenumber = table.Column<string>(nullable: true),
                    TransactionID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderHeaders", x => x.id);
                    table.ForeignKey(
                        name: "FK_OrderHeaders_AspNetUsers_userid",
                        column: x => x.userid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDatails",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    orderid = table.Column<int>(nullable: false),
                    Menuitemid = table.Column<int>(nullable: false),
                    count = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    price = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDatails", x => x.id);
                    table.ForeignKey(
                        name: "FK_OrderDatails_Menuitem_Menuitemid",
                        column: x => x.Menuitemid,
                        principalTable: "Menuitem",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDatails_OrderHeaders_orderid",
                        column: x => x.orderid,
                        principalTable: "OrderHeaders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDatails_Menuitemid",
                table: "OrderDatails",
                column: "Menuitemid");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDatails_orderid",
                table: "OrderDatails",
                column: "orderid");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeaders_userid",
                table: "OrderHeaders",
                column: "userid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDatails");

            migrationBuilder.DropTable(
                name: "OrderHeaders");
        }
    }
}
