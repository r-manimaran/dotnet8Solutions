using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Benchmark_UUID_ULID.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Table1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Table1", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Table2",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Table2", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Table3",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(26)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Table3", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Table4",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Table4", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Table5",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GetUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Table5", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Table5_CreatedOnUtc",
                table: "Table5",
                column: "CreatedOnUtc")
                .Annotation("SqlServer:Clustered", true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Table1");

            migrationBuilder.DropTable(
                name: "Table2");

            migrationBuilder.DropTable(
                name: "Table3");

            migrationBuilder.DropTable(
                name: "Table4");

            migrationBuilder.DropTable(
                name: "Table5");
        }
    }
}
