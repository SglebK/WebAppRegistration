using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppRegistration.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteToUserColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "Users");
        }
    }
}
