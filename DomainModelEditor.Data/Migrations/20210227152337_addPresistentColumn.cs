using Microsoft.EntityFrameworkCore.Migrations;

namespace DomainModelEditor.Data.SqlServer.Migrations
{
    public partial class addPresistentColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPersistentEntity",
                table: "Entities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPersistentEntity",
                table: "Entities");
        }
    }
}
