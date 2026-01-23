using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teacher_Dashboard_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddGroupId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_students_groups_GroupId",
                table: "students");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "students",
                newName: "group_id");

            migrationBuilder.RenameIndex(
                name: "IX_students_GroupId",
                table: "students",
                newName: "IX_students_group_id");

            migrationBuilder.AlterColumn<int>(
                name: "group_id",
                table: "students",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_students_groups_group_id",
                table: "students",
                column: "group_id",
                principalTable: "groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_students_groups_group_id",
                table: "students");

            migrationBuilder.RenameColumn(
                name: "group_id",
                table: "students",
                newName: "GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_students_group_id",
                table: "students",
                newName: "IX_students_GroupId");

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "students",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_students_groups_GroupId",
                table: "students",
                column: "GroupId",
                principalTable: "groups",
                principalColumn: "id");
        }
    }
}
