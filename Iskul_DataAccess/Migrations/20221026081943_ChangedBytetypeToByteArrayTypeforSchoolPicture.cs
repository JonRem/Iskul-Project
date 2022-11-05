using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iskul_DataAccess.Migrations
{
    public partial class ChangedBytetypeToByteArrayTypeforSchoolPicture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "SchoolPhoto",
                table: "EnrollDetail",
                type: "varbinary(max)",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "SchoolPhoto",
                table: "EnrollDetail",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");
        }
    }
}
