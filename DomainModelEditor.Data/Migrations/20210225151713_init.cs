using Microsoft.EntityFrameworkCore.Migrations;

namespace DomainModelEditor.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attributes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributeName = table.Column<string>(maxLength: 50, nullable: false),
                    AttributeType = table.Column<int>(nullable: false),
                    DefaultValue = table.Column<string>(maxLength: 50, nullable: true),
                    MinValue = table.Column<string>(maxLength: 20, nullable: true),
                    MaxValue = table.Column<string>(maxLength: 50, nullable: true),
                    AllowNull = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attributes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Entities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Coords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    X = table.Column<int>(nullable: false),
                    Y = table.Column<int>(nullable: false),
                    EntityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Coords_Entities_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityAttributeValue",
                columns: table => new
                {
                    EntityId = table.Column<int>(nullable: false),
                    AttributeId = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityAttributeValue", x => new { x.EntityId, x.AttributeId });
                    table.ForeignKey(
                        name: "FK_EntityAttributeValue_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "Attributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityAttributeValue_Entities_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Attributes",
                columns: new[] { "Id", "AllowNull", "AttributeName", "AttributeType", "DefaultValue", "MaxValue", "MinValue" },
                values: new object[] { 1, false, "FirstName", 1, null, "50", "5" });

            migrationBuilder.InsertData(
                table: "Entities",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Car" });

            migrationBuilder.InsertData(
                table: "Entities",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Wheels" });

            migrationBuilder.InsertData(
                table: "Coords",
                columns: new[] { "Id", "EntityId", "X", "Y" },
                values: new object[] { 1, 1, 100, 100 });

            migrationBuilder.InsertData(
                table: "Coords",
                columns: new[] { "Id", "EntityId", "X", "Y" },
                values: new object[] { 2, 2, 200, 200 });

            migrationBuilder.CreateIndex(
                name: "IX_Coords_EntityId",
                table: "Coords",
                column: "EntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EntityAttributeValue_AttributeId",
                table: "EntityAttributeValue",
                column: "AttributeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coords");

            migrationBuilder.DropTable(
                name: "EntityAttributeValue");

            migrationBuilder.DropTable(
                name: "Attributes");

            migrationBuilder.DropTable(
                name: "Entities");
        }
    }
}
