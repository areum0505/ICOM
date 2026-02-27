using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICOM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCodeTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "group_codes",
                columns: table => new
                {
                    group_code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    group_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    creator_id = table.Column<int>(type: "int", nullable: false),
                    delete_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_use = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_group_codes", x => x.group_code);
                });

            migrationBuilder.CreateTable(
                name: "detail_codes",
                columns: table => new
                {
                    detail_code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    group_code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    detail_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    creator_id = table.Column<int>(type: "int", nullable: false),
                    delete_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_use = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detail_codes", x => x.detail_code);
                    table.ForeignKey(
                        name: "FK_detail_codes_group_codes_group_code",
                        column: x => x.group_code,
                        principalTable: "group_codes",
                        principalColumn: "group_code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_detail_codes_group_code",
                table: "detail_codes",
                column: "group_code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "detail_codes");

            migrationBuilder.DropTable(
                name: "group_codes");
        }
    }
}
