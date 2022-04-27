using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class ver14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "Payslips");

            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "Payslips");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Payslips");

            migrationBuilder.DropColumn(
                name: "UpdateBy",
                table: "Payslips");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "Payslips");

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CreateBy", "CreateTime", "Description", "Manager", "Name", "UpdateBy", "UpdateTime" },
                values: new object[] { (short)-2, "PAHTEST\\41776d", new DateTime(2022, 4, 27, 21, 34, 36, 16, DateTimeKind.Local).AddTicks(6067), "HR", "Dennis", "HR", "PAHTEST\\41776d", new DateTime(2022, 4, 27, 21, 34, 36, 16, DateTimeKind.Local).AddTicks(6067) });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CreateBy", "CreateTime", "Description", "Manager", "Name", "UpdateBy", "UpdateTime" },
                values: new object[] { (short)-1, "PAHTEST\\41776d", new DateTime(2022, 4, 27, 21, 34, 36, 16, DateTimeKind.Local).AddTicks(5024), "IT", "Mullar", "IT", "PAHTEST\\41776d", new DateTime(2022, 4, 27, 21, 34, 36, 16, DateTimeKind.Local).AddTicks(5024) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: (short)-2);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: (short)-1);

            migrationBuilder.AddColumn<string>(
                name: "CreateBy",
                table: "Payslips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTime",
                table: "Payslips",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Payslips",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "UpdateBy",
                table: "Payslips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                table: "Payslips",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
