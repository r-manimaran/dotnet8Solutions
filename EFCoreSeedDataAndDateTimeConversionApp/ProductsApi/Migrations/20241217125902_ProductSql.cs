using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductsApi.Migrations
{
    /// <inheritdoc />
    public partial class ProductSql : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Products " +
                "(Name, CreatedDate) " +
                "VALUES " +
                "('BOOK', DATETIME('now'))");

            migrationBuilder.Sql("INSERT INTO Products " +
               "(Name, CreatedDate) " +
               "VALUES " +
               "('LAPTOP', DATETIME('now'))");

            migrationBuilder.Sql("INSERT INTO Products " +
              "(Name, CreatedDate) " +
              "VALUES " +
              "('MobilePhone', DATETIME('now'))");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
