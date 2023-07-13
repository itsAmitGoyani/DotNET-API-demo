using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sportal.Infrastructure.Persistence.Migrations
{
    public partial class UpdateVisitTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Visitors_Departments_DepartmentId",
                table: "Visitors");

            migrationBuilder.DropIndex(
                name: "IX_Visitors_DepartmentId",
                table: "Visitors");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Visitors");

            migrationBuilder.AlterColumn<Guid>(
                name: "ExitBy",
                table: "Visits",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExitAtUtc",
                table: "Visits",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "Visits",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Visits_DepartmentId",
                table: "Visits",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Visits_Departments_DepartmentId",
                table: "Visits",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Visits_Departments_DepartmentId",
                table: "Visits");

            migrationBuilder.DropIndex(
                name: "IX_Visits_DepartmentId",
                table: "Visits");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Visits");

            migrationBuilder.AlterColumn<Guid>(
                name: "ExitBy",
                table: "Visits",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExitAtUtc",
                table: "Visits",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "Visitors",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Visitors_DepartmentId",
                table: "Visitors",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Visitors_Departments_DepartmentId",
                table: "Visitors",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
