using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sportal.Infrastructure.Persistence.Migrations
{
    public partial class AddVisitorTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Visitors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    CNIC = table.Column<string>(type: "text", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CardIssue = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CardExpiry = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Childern = table.Column<int>(type: "integer", nullable: true),
                    Vahicle = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visitors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Visitors_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Visits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VisitorId = table.Column<Guid>(type: "uuid", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    EntryBy = table.Column<Guid>(type: "uuid", nullable: false),
                    EntryAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExitBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ExitAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Visits_Visitors_VisitorId",
                        column: x => x.VisitorId,
                        principalTable: "Visitors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Visitors_DepartmentId",
                table: "Visitors",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_VisitorId",
                table: "Visits",
                column: "VisitorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Visits");

            migrationBuilder.DropTable(
                name: "Visitors");
        }
    }
}
