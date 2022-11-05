using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iskul_DataAccess.Migrations
{
    public partial class CorrectionsToForeignKeysInEnrollHeaderandDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SchoolPhoto",
                table: "EnrollDetail",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "SchoolPhoto",
                table: "EnrollDetail",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
