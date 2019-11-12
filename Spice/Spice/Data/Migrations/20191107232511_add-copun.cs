using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Spice.Data.Migrations
{
    public partial class addcopun : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "Copuns",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "CouponType",
                table: "Copuns",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Discount",
                table: "Copuns",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Copuns",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "MinimumAmount",
                table: "Copuns",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<byte[]>(
                name: "Picture",
                table: "Copuns",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CouponType",
                table: "Copuns");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Copuns");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Copuns");

            migrationBuilder.DropColumn(
                name: "MinimumAmount",
                table: "Copuns");

            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Copuns");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Copuns",
                newName: "name");
        }
    }
}
