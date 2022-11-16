using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iskul_DataAccess.Migrations
{
    public partial class EditedEnrollHeaderandDetailFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnrollDetail_School_SchoolId",
                table: "EnrollDetail");

            migrationBuilder.DropIndex(
                name: "IX_EnrollDetail_SchoolId",
                table: "EnrollDetail");

            migrationBuilder.DropColumn(
                name: "EnrollDate",
                table: "EnrollHeader");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "EnrollDetail");

            migrationBuilder.AddColumn<bool>(
                name: "DetailRecOpen",
                table: "EnrollHeader",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LastDetailRec",
                table: "EnrollHeader",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SchoolId",
                table: "EnrollHeader",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EnrollDate",
                table: "EnrollDetail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_EnrollHeader_SchoolId",
                table: "EnrollHeader",
                column: "SchoolId");

            migrationBuilder.AddForeignKey(
                name: "FK_EnrollHeader_School_SchoolId",
                table: "EnrollHeader",
                column: "SchoolId",
                principalTable: "School",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnrollHeader_School_SchoolId",
                table: "EnrollHeader");

            migrationBuilder.DropIndex(
                name: "IX_EnrollHeader_SchoolId",
                table: "EnrollHeader");

            migrationBuilder.DropColumn(
                name: "DetailRecOpen",
                table: "EnrollHeader");

            migrationBuilder.DropColumn(
                name: "LastDetailRec",
                table: "EnrollHeader");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "EnrollHeader");

            migrationBuilder.DropColumn(
                name: "EnrollDate",
                table: "EnrollDetail");

            migrationBuilder.AddColumn<DateTime>(
                name: "EnrollDate",
                table: "EnrollHeader",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "SchoolId",
                table: "EnrollDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EnrollDetail_SchoolId",
                table: "EnrollDetail",
                column: "SchoolId");

            migrationBuilder.AddForeignKey(
                name: "FK_EnrollDetail_School_SchoolId",
                table: "EnrollDetail",
                column: "SchoolId",
                principalTable: "School",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
