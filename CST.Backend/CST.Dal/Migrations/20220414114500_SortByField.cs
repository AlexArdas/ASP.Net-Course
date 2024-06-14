using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CST.Dal.Migrations
{
    public partial class SortByField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SortBy",
                table: "Report",
                newName: "SortOrder");

            migrationBuilder.AddColumn<string>(
                name: "SortByField",
                table: "Report",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SortByField",
                table: "Report");

            migrationBuilder.RenameColumn(
                name: "SortOrder",
                table: "Report",
                newName: "SortBy");
        }
    }
}
