using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity_Project.Migrations
{
    /// <inheritdoc />
    public partial class map_NormalizedEmail_Prop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "Users");

        }
    }
}
