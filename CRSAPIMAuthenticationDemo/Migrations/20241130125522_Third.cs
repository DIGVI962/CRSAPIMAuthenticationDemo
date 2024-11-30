using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRSAPIMAuthenticationDemo.Migrations
{
    /// <inheritdoc />
    public partial class Third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MacAddress",
                table: "TokenRecords");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MacAddress",
                table: "TokenRecords",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
