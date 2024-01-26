using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity_Project.Migrations
{
    /// <inheritdoc />
    public partial class addBirthDate_ToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Users");
        }
    }
}
