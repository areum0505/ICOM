using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICOM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameCodeTablesToPascalCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_detail_codes_group_codes_group_code",
                table: "detail_codes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_group_codes",
                table: "group_codes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_detail_codes",
                table: "detail_codes");

            migrationBuilder.RenameTable(
                name: "group_codes",
                newName: "GroupCode");

            migrationBuilder.RenameTable(
                name: "detail_codes",
                newName: "DetailCode");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "GroupCode",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "is_use",
                table: "GroupCode",
                newName: "IsUse");

            migrationBuilder.RenameColumn(
                name: "group_name",
                table: "GroupCode",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "delete_date",
                table: "GroupCode",
                newName: "DeleteDate");

            migrationBuilder.RenameColumn(
                name: "creator_id",
                table: "GroupCode",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "create_date",
                table: "GroupCode",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "group_code",
                table: "GroupCode",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "DetailCode",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "is_use",
                table: "DetailCode",
                newName: "IsUse");

            migrationBuilder.RenameColumn(
                name: "group_code",
                table: "DetailCode",
                newName: "GroupCode");

            migrationBuilder.RenameColumn(
                name: "detail_name",
                table: "DetailCode",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "delete_date",
                table: "DetailCode",
                newName: "DeleteDate");

            migrationBuilder.RenameColumn(
                name: "creator_id",
                table: "DetailCode",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "create_date",
                table: "DetailCode",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "detail_code",
                table: "DetailCode",
                newName: "Code");

            migrationBuilder.RenameIndex(
                name: "IX_detail_codes_group_code",
                table: "DetailCode",
                newName: "IX_DetailCode_GroupCode");

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
            migrationBuilder.DropForeignKey(
                name: "FK_DetailCode_GroupCode_GroupCode",
                table: "DetailCode");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupCode",
                table: "GroupCode");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DetailCode",
                table: "DetailCode");

            migrationBuilder.RenameTable(
                name: "GroupCode",
                newName: "group_codes");

            migrationBuilder.RenameTable(
                name: "DetailCode",
                newName: "detail_codes");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "group_codes",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "group_codes",
                newName: "group_name");

            migrationBuilder.RenameColumn(
                name: "IsUse",
                table: "group_codes",
                newName: "is_use");

            migrationBuilder.RenameColumn(
                name: "DeleteDate",
                table: "group_codes",
                newName: "delete_date");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "group_codes",
                newName: "creator_id");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "group_codes",
                newName: "create_date");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "group_codes",
                newName: "group_code");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "detail_codes",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "detail_codes",
                newName: "detail_name");

            migrationBuilder.RenameColumn(
                name: "IsUse",
                table: "detail_codes",
                newName: "is_use");

            migrationBuilder.RenameColumn(
                name: "GroupCode",
                table: "detail_codes",
                newName: "group_code");

            migrationBuilder.RenameColumn(
                name: "DeleteDate",
                table: "detail_codes",
                newName: "delete_date");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "detail_codes",
                newName: "creator_id");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "detail_codes",
                newName: "create_date");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "detail_codes",
                newName: "detail_code");

            migrationBuilder.RenameIndex(
                name: "IX_DetailCode_GroupCode",
                table: "detail_codes",
                newName: "IX_detail_codes_group_code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_group_codes",
                table: "group_codes",
                column: "group_code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_detail_codes",
                table: "detail_codes",
                column: "detail_code");

            migrationBuilder.AddForeignKey(
                name: "FK_detail_codes_group_codes_group_code",
                table: "detail_codes",
                column: "group_code",
                principalTable: "group_codes",
                principalColumn: "group_code",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
