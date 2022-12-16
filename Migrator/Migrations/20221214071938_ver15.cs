using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class ver15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Route = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    Method = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    Parameters = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Request = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestLog", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: (short)-2,
                columns: new[] { "CreateBy", "CreateTime", "UpdateBy", "UpdateTime" },
                values: new object[] { "PAHTEST\\ditzapj001", new DateTime(2022, 12, 14, 15, 19, 38, 171, DateTimeKind.Local).AddTicks(8412), "PAHTEST\\ditzapj001", new DateTime(2022, 12, 14, 15, 19, 38, 171, DateTimeKind.Local).AddTicks(8412) });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: (short)-1,
                columns: new[] { "CreateBy", "CreateTime", "UpdateBy", "UpdateTime" },
                values: new object[] { "PAHTEST\\ditzapj001", new DateTime(2022, 12, 14, 15, 19, 38, 171, DateTimeKind.Local).AddTicks(6008), "PAHTEST\\ditzapj001", new DateTime(2022, 12, 14, 15, 19, 38, 171, DateTimeKind.Local).AddTicks(6008) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestLog");

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: (short)-2,
                columns: new[] { "CreateBy", "CreateTime", "UpdateBy", "UpdateTime" },
                values: new object[] { "PAHTEST\\41776d", new DateTime(2022, 4, 27, 21, 34, 36, 16, DateTimeKind.Local).AddTicks(6067), "PAHTEST\\41776d", new DateTime(2022, 4, 27, 21, 34, 36, 16, DateTimeKind.Local).AddTicks(6067) });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: (short)-1,
                columns: new[] { "CreateBy", "CreateTime", "UpdateBy", "UpdateTime" },
                values: new object[] { "PAHTEST\\41776d", new DateTime(2022, 4, 27, 21, 34, 36, 16, DateTimeKind.Local).AddTicks(5024), "PAHTEST\\41776d", new DateTime(2022, 4, 27, 21, 34, 36, 16, DateTimeKind.Local).AddTicks(5024) });
        }
    }
}
