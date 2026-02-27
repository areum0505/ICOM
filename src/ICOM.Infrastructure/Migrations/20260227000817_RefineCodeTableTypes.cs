using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICOM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefineCodeTableTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "GroupCode");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "DetailCode");

            // PK/FK 참조 컬럼 타입 변경 시 MSSQL 제약 순서 준수:
            // FK → PK DROP → 컬럼 타입 변경 → PK → FK 재추가

            migrationBuilder.DropForeignKey(
                name: "FK_DetailCode_GroupCode_GroupCode",
                table: "DetailCode");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupCode",
                table: "GroupCode");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DetailCode",
                table: "DetailCode");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "GroupCode",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "GroupCode",
                table: "DetailCode",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "DetailCode",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupCode",
                table: "GroupCode",
                column: "Code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetailCode",
                table: "DetailCode",
                column: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_DetailCode_GroupCode_GroupCode",
                table: "DetailCode",
                column: "GroupCode",
                principalTable: "GroupCode",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "GroupCode",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "GroupCode",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "GroupCode",
                table: "DetailCode",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "DetailCode",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "DetailCode",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
